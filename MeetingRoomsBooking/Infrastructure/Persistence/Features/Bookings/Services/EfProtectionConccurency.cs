using MeetingRoomsBooking.Features.Abstractions.Persistence.UnitOfWork;
using MeetingRoomsBooking.Features.Bookings.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Services
{
    public sealed class EfProtectionConccurency : IProtectionConccurency
    {
        private readonly IUnitOfWork _uow;

        public EfProtectionConccurency(IUnitOfWork uow) => _uow = uow;


        public async Task<bool> TrySaveAsync(CancellationToken ct)
        {
            try
            {
                await _uow.SaveChangesAsync(ct);
                return true;
            }

            catch (DbUpdateConcurrencyException)
            {
                _uow.ClearTracking();
                return false;
            }
        }
    }
}
