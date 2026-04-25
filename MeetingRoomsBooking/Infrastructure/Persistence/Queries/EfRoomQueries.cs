using MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;
using MeetingRoomsBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Queries
{
    public class EfRoomQueries : IRoomQueries
    {
        private readonly BookingDbContext _db;

        public EfRoomQueries(BookingDbContext db) => _db = db;


        public async Task<bool> IsExistsByNameAsync(RoomName name, CancellationToken ct)
            => await _db.Rooms
            .AnyAsync(r => r.Name == name, ct);
    }
}
