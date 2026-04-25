using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Infrastructure.Data;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Repositories
{
    public sealed class EfRoomRepository : IRoomRepository
    {
        private readonly BookingDbContext _db;
        
        public EfRoomRepository(BookingDbContext db) => _db = db;


        public void Add(Room entity) => _db.Add(entity);
    }
}
