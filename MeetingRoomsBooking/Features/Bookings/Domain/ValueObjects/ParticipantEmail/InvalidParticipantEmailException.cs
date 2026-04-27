using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.ParticipantEmail
{
    public sealed class InvalidParticipantEmailException : DomainException
    {
        private InvalidParticipantEmailException(string value, string reason) : base(
            code: "INVALID_PARTICIPANT_EMAIL",
            message: $"Incorrect participant email: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidParticipantEmailException MustBeNotEmpty(string value) =>
            new(value, "The participant email cannot be empty.");

        public static InvalidParticipantEmailException InvalidFormat(string value) =>
            new(value, $"The participant email has an incorrect format.");
        public static InvalidParticipantEmailException MustBeLessThan(string value, int maxLength) =>
            new(value, $"The length of the participant email must be less than or equal to: {maxLength}");
    }
}
