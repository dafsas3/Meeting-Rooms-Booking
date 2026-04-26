namespace MeetingRoomsBooking.Features.Bookings.Application.Commands
{
    public sealed record CreateBookingRequestCommand(
        string RoomName,
        Guid EmployeeId,
        DateTime StartedAtUtc,
        DateTime EndAtUtc,
        string Purpose,
        string Role,
        List<string> Emails);
}
