using FluentValidation;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;
using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms
{
    public sealed class GetRoomsHandler
    {
        private readonly BookingDbContext _db;
        private IValidator<GetRoomsQuery> _validator;

        public GetRoomsHandler(BookingDbContext db, IValidator<GetRoomsQuery> validator)
        {
            _db = db;
            _validator = validator;
        }


        public async Task<Result<List<RoomReadModel>>> Handle(GetRoomsQuery query, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(query, ct);

            if (!validation.IsValid)
            {
                return Result<List<RoomReadModel>>.BadRequest(
                    "VALIDATION_ERROR", validation.Errors.First().ErrorMessage);
            }

            IQueryable<Room> rooms = _db.Rooms.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                rooms = rooms.Where(r => r.Name == RoomName.Create(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Location))
            {
                rooms = rooms.Where(r => r.Location == RoomLocation.Create(query.Location));
            }

            if (query.MinCapacity is not null)
            {
                var minCapacity = RoomCapacity.Create(query.MinCapacity.Value);
                rooms = rooms.Where(r => r.Capacity >= minCapacity);
            }

            if (query.IsActive is not null)
            {
                rooms = rooms.Where(r => r.IsActive == query.IsActive);
            }

            var result = await rooms.
                Select(r => new RoomReadModel
                {
                    Id = r.Id.Value,
                    Name = r.Name.Value,
                    Location = r.Location.Value,
                    Capacity = r.Capacity.Value,
                    IsActive = r.IsActive
                })
                .ToListAsync(ct);

            return Result<List<RoomReadModel>>.Ok(result);
        }

    }
}
