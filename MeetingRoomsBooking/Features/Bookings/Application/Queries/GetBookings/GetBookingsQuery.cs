namespace MeetingRoomsBooking.Features.Bookings.Application.Queries.GetBookings
{
    public sealed record GetBookingsQuery(
        int? RoomId,
        DateTime? From,
        DateTime? To,
        string? Status);
}
