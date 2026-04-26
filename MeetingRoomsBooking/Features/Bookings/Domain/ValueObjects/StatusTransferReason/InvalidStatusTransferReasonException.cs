using MeetingRoomsBooking.Features.Abstractions.Domain;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose;

namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason
{
    public sealed class InvalidStatusTransferReasonException : DomainException
    {
        private InvalidStatusTransferReasonException(string value, string reason) : base(
            code: "INVALID_BOOKING_STATUS_TRANSFER_REASON",
            message: $"Incorrect booking status transfer reason: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidStatusTransferReasonException MustBeNotEmpty(string value) =>
            new(value, "The booking status transfer reason cannot be empty.");

        public static InvalidStatusTransferReasonException MustBeGreaterThan(string value, int minLength) =>
            new(value, $"The length of the booking status transfer reason " +
                $"must be greater than or equal to: {minLength}");
        public static InvalidStatusTransferReasonException MustBeLessThan(string value, int maxLength) =>
            new(value, $"The length of the booking status transfer reason " +
                $"must be less than or equal to: {maxLength}");
    }
}
