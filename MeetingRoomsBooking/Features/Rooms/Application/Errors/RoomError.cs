using MeetingRoomsBooking.Features.Abstractions.Common.Errors;

namespace MeetingRoomsBooking.Features.Rooms.Application.Errors
{
    public static class RoomError
    {
        public static ApiError NameAlreadyExistsInThisLocation(string room, string location)
            => new(Code: "ROOM_NAME_IN_THE_LOCATION_ALREADY_EXISTS",
                Message: $"The name already exists in this location: {room}, location: {location}");
    }
}
