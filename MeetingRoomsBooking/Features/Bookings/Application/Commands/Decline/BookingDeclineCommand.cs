namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Decline
{
    public sealed record BookingDeclineCommand(
        int BookingRequestId,
        string Reason);
}
