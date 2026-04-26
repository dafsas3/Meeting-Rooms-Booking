using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId
{
    public sealed class InvalidBookingRequestIdException : DomainException
    {
        private InvalidBookingRequestIdException(int value, string reason) : base(
            code: "INVALID_BOOKING_REQUEST_ID",
            message: $"Incorrect booking request ID: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidBookingRequestIdException MustBeGreaterThanZero(int value) =>
            new(value, "The booking request ID must be greater than zero.");
    }
}
