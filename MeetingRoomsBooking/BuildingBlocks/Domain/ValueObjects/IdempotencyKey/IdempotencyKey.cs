namespace MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey
{
    public sealed record IdempotencyKey
    {
        public Guid Value { get; }

        private IdempotencyKey(Guid value) => Value = value;

        public static IdempotencyKey Create(Guid value)
        {
            if (value == Guid.Empty)
                throw InvalidIdempotencyKeyException.MustBeNotEmpty(value);

            return new IdempotencyKey(value);
        }
    }
}
