namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose
{
    public sealed record MeetingPurpose
    {
        public string Value { get; }
        private const int MinLength = 7;
        private const int MaxLength = 300;

        private MeetingPurpose(string value) => Value = value;

        public static MeetingPurpose Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw InvalidMeetingPurposeException.MustBeNotEmpty(value);

            var normalized = value.Trim();

            if (normalized.Length < MinLength)
                throw InvalidMeetingPurposeException.MustBeGreaterThan(value, MinLength);

            if (normalized.Length > MaxLength)
                throw InvalidMeetingPurposeException.MustBeLessThan(value, MaxLength);

            return new MeetingPurpose(normalized);
        }
    }
}
