using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose
{
    public sealed class InvalidMeetingPurposeException : DomainException
    {
        private InvalidMeetingPurposeException(string value, string reason) : base(
            code: "INVALID_MEETING_PURPOSE",
            message: $"Incorrect meeting purpose: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidMeetingPurposeException MustBeNotEmpty(string value) =>
            new(value, "The meeting purpose cannot be empty.");

        public static InvalidMeetingPurposeException MustBeGreaterThan(string value, int minLength) =>
            new(value, $"The length of the meeting purpose must be greater than or equal to: {minLength}");
        public static InvalidMeetingPurposeException MustBeLessThan(string value, int maxLength) =>
            new(value, $"The length of the meeting purpose must be less than or equal to: {maxLength}");
    }
}
