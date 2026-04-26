using MimeKit;

namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.ParticipantEmail
{
    public sealed record ParticipantEmail
    {
        public string Value { get; }
        private const int MaxLength = 254;

        private ParticipantEmail(string value) => Value = value;

        public static ParticipantEmail Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw InvalidParticipantEmailException.MustBeNotEmpty(value);

            var normalized = value.Trim();

            if (normalized.Length > MaxLength)
                throw InvalidParticipantEmailException.MustBeLessThan(value, MaxLength);

            if (!MailboxAddress.TryParse(normalized, out var emailAddress))
                throw InvalidParticipantEmailException.InvalidFormat(value);

            if (emailAddress.Address != normalized)
                throw InvalidParticipantEmailException.InvalidFormat(value);

            return new ParticipantEmail(normalized);
        }
    }
}
