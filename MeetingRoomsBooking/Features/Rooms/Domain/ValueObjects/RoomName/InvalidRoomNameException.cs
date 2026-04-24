using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName
{
    public sealed class InvalidRoomNameException : DomainException
    {
        private InvalidRoomNameException(string value, string reason) : base(
            code: "INVALID_ROOM_NAME",
            message: $"Incorrect room name: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidRoomNameException MustBeNotEmpty(string value) =>
            new(value, "The room name cannot be empty.");

        public static InvalidRoomNameException LengthMustBeGreaterThan(string value, int minLength)
            => new(value, $"The length of the room name must be greater than: {minLength}");

        public static InvalidRoomNameException LengthMustBeLessThan(string value, int maxLength)
            => new(value, $"The length of the room name must be less than: {maxLength}");
    }
}
