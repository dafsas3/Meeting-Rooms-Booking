using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Rooms.Domain.Ids
{
    public sealed class InvalidRoomIdException : DomainException
    {
        private InvalidRoomIdException(int value, string reason, int statusCode) : base(
            code: "INVALID_ROOM_ID",
            message: $"Incorrect room ID: \"{value}\". Reason: {reason}.",
            statusCode: statusCode,
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidRoomIdException MustBeGreaterThanZero(int value) =>
            new(value, "The room ID must be greater than zero.", 400);
    }
}
