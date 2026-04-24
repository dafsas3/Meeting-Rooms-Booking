using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName
{
    public sealed class InvalidRoomNameException : DomainException
    {
        private InvalidRoomNameException(string value, string reason, int statusCode) : base(
            code: "INVALID_ROOM_NAME",
            message: $"Incorrect room name: \"{value}\". Reason: {reason}.",
            statusCode: statusCode,
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidRoomNameException MustBeNotEmpty(string value) =>
            new(value, "The room name cannot be empty.", 400);

        public static InvalidRoomNameException LengthMustBeGreaterThan(string value, int minLength)
            => new(value, $"The length of the room name must be greater than: {minLength}", 400);

        public static InvalidRoomNameException LengthMustBeLessThan(string value, int maxLength)
            => new(value, $"The length of the room name must be less than: {maxLength}", 400);
    }
}
