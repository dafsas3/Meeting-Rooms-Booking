using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingHistoryId
{
    public sealed class InvalidBookingHistoryIdException : DomainException
    {
        private InvalidBookingHistoryIdException(int value, string reason) : base(
            code: "INVALID_BOOKING_HISTORY_ID",
            message: $"Incorrect booking history ID: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }

        public static InvalidBookingHistoryIdException MustBeGreaterThanZero(int value) =>
            new(value, $"The booking history ID must be greater than zero.");
    }
}
