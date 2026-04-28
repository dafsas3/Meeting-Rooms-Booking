namespace MeetingRoomsBooking.Features.Bookings.Api.Requests
{
    public sealed class BookingCancelRequest
    {
        public required string Reason { get; set; }
    }
}
