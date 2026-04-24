using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation
{
    public sealed class InvalidRoomLocationException : DomainException
    {
        private InvalidRoomLocationException(string value, string reason, int statusCode) : base(
            code: "INVALID_ROOM_LOCATION",
            message: $"Incorrect room location: \"{value}\". Reason: {reason}.",
            statusCode: statusCode,
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidRoomLocationException MustBeNotEmpty(string value) =>
            new(value, "The room location cannot be empty.", 400);

        public static InvalidRoomLocationException LengthMustBeGreaterThan(string value, int minLength)
            => new(value, $"The length of the room location must be greater than: {minLength}", 400);

        public static InvalidRoomLocationException LengthMustBeLessThan(string value, int maxLength)
            => new(value, $"The length of the room location must be less than: {maxLength}", 400);
    }
}
