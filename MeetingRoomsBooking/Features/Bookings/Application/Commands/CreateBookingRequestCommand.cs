namespace MeetingRoomsBooking.Features.Bookings.Application.Commands
{
    public sealed record CreateBookingRequestCommand(
        int RoomId,
        DateTime StartedAtUtc,
        DateTime EndAtUtc,
        string Purpose,
        List<string> Emails);
}
