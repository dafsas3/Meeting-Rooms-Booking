using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Rooms.Application.Errors;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;

namespace MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom
{
    public sealed class CreateRoomHandler
    {
        private readonly IRoomQueries _roomQueries;

        public CreateRoomHandler(IRoomQueries roomQueries)
        {
            _roomQueries = roomQueries;
        }

        public async Task<Result<CreateRoomResponse>> Handle(CreateRoomCommand cmd, CancellationToken ct)
        {
            var name = RoomName.Create(cmd.Name);

            if (await _roomQueries.IsExistsByNameAsync(name, ct))
            {
                var error = RoomError.NameAlreadyExists(name.Value);
                return Result<CreateRoomResponse>.Conflict(error.Code, error.Message);
            }


        }

    }
}
