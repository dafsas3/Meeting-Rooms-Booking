namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Cancel
{
    public sealed record BookingCancelCommand(
        int BookingRequestId,
        string Reason);
}
