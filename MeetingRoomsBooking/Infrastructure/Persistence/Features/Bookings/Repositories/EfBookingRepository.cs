using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Repositories
{
    public sealed class EfBookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _db;

        public EfBookingRepository(BookingDbContext db) => _db = db;


        public void Add(BookingRequest entity) => _db.Add(entity);


        public async Task<BookingRequest?> GetEntityByIdAsync(
            BookingRequestId id,
            CancellationToken ct)
            => await _db.BookingRequests
            .FirstOrDefaultAsync(b => b.Id  == id, ct);
    }
}
