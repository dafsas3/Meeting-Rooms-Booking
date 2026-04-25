using MeetingRoomsBooking.Features.Abstractions.Common.Errors;

namespace MeetingRoomsBooking.Features.Rooms.Application.Errors
{
    public static class RoomError
    {
        public static ApiError NameAlreadyExists(string value)
            => new("ROOM_NAME_ALREADY_EXISTS", $"The room name is already exists: {value}.");
    }
}
