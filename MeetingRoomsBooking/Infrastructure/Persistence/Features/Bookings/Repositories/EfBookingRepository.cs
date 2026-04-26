using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Repositories
{
    public sealed class EfBookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _db;

        public EfBookingRepository(BookingDbContext db) => _db = db;


        public void Add(BookingRequest entity) => _db.Add(entity);
    }
}
