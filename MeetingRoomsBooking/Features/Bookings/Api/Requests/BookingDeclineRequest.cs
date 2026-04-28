namespace MeetingRoomsBooking.Features.Bookings.Api.Requests
{
    public sealed class BookingDeclineRequest
    {
        public required string Reason { get; set; }
    }
}
