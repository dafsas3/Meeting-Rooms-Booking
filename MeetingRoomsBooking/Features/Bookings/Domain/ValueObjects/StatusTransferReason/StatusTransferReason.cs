namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason
{
    public sealed record StatusTransferReason
    {
        public string Value { get; }
        private const int MinLength = 10;
        private const int MaxLength = 200;

        private StatusTransferReason(string value) => Value = value;

        public static StatusTransferReason Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw InvalidStatusTransferReasonException.MustBeNotEmpty(value);

            if (value.Length < MinLength)
                throw InvalidStatusTransferReasonException.MustBeGreaterThan(value, MinLength);

            if (value.Length > MaxLength)
                throw InvalidStatusTransferReasonException.MustBeLessThan(value, MaxLength);

            return new StatusTransferReason(value);
        }
    }
}
