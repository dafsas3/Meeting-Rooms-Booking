using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Features.Rooms.Queries
{
    public class EfRoomQueries : IRoomQueries
    {
        private readonly BookingDbContext _db;

        public EfRoomQueries(BookingDbContext db) => _db = db;


        public async Task<bool> IsExistsByNameAndLocationAsync(
            RoomName name,
            RoomLocation location,
            CancellationToken ct)
            =>
            await _db.Rooms
            .AnyAsync(r => r.Name == name && r.Location == location, ct);

        public async Task<RoomReadModel?> GetById(RoomId roomId, CancellationToken ct)
        {
            return await _db.Rooms
               .AsNoTracking()
               .Where(r => r.Id == roomId)
               .Select(r => new RoomReadModel
               {
                   Id = r.Id.Value,
                   Name = r.Name.Value,
                   Capacity = r.Capacity.Value,
                   Location = r.Location.Value,
                   IsActive = r.IsActive
               })
               .FirstOrDefaultAsync(ct);
        }
    }
}
