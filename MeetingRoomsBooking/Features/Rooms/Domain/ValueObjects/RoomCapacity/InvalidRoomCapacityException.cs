using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity
{
    public sealed class InvalidRoomCapacityException : DomainException
    {
        private InvalidRoomCapacityException(int value, string reason, int statusCode) : base(
            code: "INVALID_ROOM_CAPACITY",
            message: $"Incorrect room capacity: \"{value}\". Reason: {reason}.",
            statusCode: statusCode,
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidRoomCapacityException MustBeGreaterThanZero(int value) =>
            new(value, "The room capacity must be greater than zero.", 400);

        public static InvalidRoomCapacityException CapacityMustBeLessThan(int value, int maxCapacity)
            => new(value, $"The room capacity must be less than: {maxCapacity}", 400);
    }
}
