using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey
{
    public sealed class InvalidIdempotencyKeyException : DomainException
    {
        private InvalidIdempotencyKeyException(Guid value, string reason) : base(
            code: "INVALID_IDEMPOTENCY_KEY",
            message: $"Incorrect idempotency key: {value}. Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidIdempotencyKeyException MustBeNotEmpty(Guid value) =>
            new(value, "The idempotency key cannot by empty.");
    }
}
