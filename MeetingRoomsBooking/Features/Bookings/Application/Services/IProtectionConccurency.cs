namespace MeetingRoomsBooking.Features.Bookings.Application.Services
{
    public interface IProtectionConccurency
    {
        Task<bool> TrySaveAsync(CancellationToken ct);
    }
}
