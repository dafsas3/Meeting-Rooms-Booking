using MeetingRoomsBooking.Features.Abstractions.Common.Errors;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Persistence.UnitOfWork;
using MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Rooms.Application.Errors;
using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;
using MeetingRoomsBooking.Infrastructure.Data;
using MeetingRoomsBooking.Infrastructure.Persistence.DbExtensions;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom
{
    public sealed class CreateRoomHandler
    {
        private readonly IRoomQueries _roomQueries;
        private readonly IRoomRepository _roomsRepository;
        private readonly IUnitOfWork _uow;

        public CreateRoomHandler(
            IRoomQueries roomQueries,
            IUnitOfWork uow,
            IRoomRepository repository)
        {
            _roomQueries = roomQueries;
            _uow = uow;
            _roomsRepository = repository;
        }

        public async Task<Result<CreateRoomResponse>> Handle(CreateRoomCommand cmd, CancellationToken ct)
        {
            var preparedCtx = Prepare(cmd);

            if (await _roomQueries.IsExistsByNameAndLocationAsync(
                preparedCtx.Name,
                preparedCtx.Location,
                ct))
            {
                var error = ToApiError(preparedCtx.Name.Value, preparedCtx.Location.Value);
                return Result<CreateRoomResponse>.Conflict(error.Code, error.Message);
            }

            var newRoom = Create(preparedCtx, cmd);
            _roomsRepository.Add(newRoom);

            try
            {
                await _uow.SaveChangesAsync(ct);
                return Result<CreateRoomResponse>.Created(ToResponse(newRoom));
            }
            
            catch (DbUpdateException ex)
            when (ex.TryGetUniqueConstraintName(out var key) &&
            key is DatabaseConstants.UniqueRoomIndexes.RoomNameAndLocation)
            {
                _uow.ClearTracking();

                var error = ToApiError(preparedCtx.Name.Value, preparedCtx.Location.Value);

                return Result<CreateRoomResponse>.Conflict(error.Code, error.Message);
            }
        }


        private static Room Create(PreparedRoomData data, CreateRoomCommand cmd)
            => Room.Create(
                data.Name,
                data.Capacity,
                data.Location,
                cmd.IsActive);


        private static ApiError ToApiError(string name, string location)
            => RoomError.NameAlreadyExistsInThisLocation(name, location);
            


        private static CreateRoomResponse ToResponse(Room entity)
            => new()
            {
                Id = entity.Id.Value,
                Name = entity.Name.Value,
                Location = entity.Location.Value,
                ReqCapacity = entity.Capacity.Value,
                IsActive = entity.IsActive
            };


        private static PreparedRoomData Prepare(CreateRoomCommand cmd)
        {
            return new(
                RoomName.Create(cmd.Name),
                RoomCapacity.Create(cmd.ReqCapacity),
                RoomLocation.Create(cmd.Location));
        }


        private record PreparedRoomData(
            RoomName Name,
            RoomCapacity Capacity,
            RoomLocation Location);

    }
}
