using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;
using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity;
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

        public async Task<RoomReadModel?> GetReadByIdAsync(RoomId roomId, CancellationToken ct)
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


        public async Task<Room?> GetEntityByIdAsync(RoomId id, CancellationToken ct)
            => await _db.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);


        public async Task<List<RoomReadModel>> GetByFiltersAsync(
            GetRoomsQuery query, CancellationToken ct)
        {
            IQueryable<Room> rooms = _db.Rooms.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Name))
                rooms = rooms.Where(r => r.Name == RoomName.Create(query.Name));

            if (!string.IsNullOrWhiteSpace(query.Location))
                rooms = rooms.Where(r => r.Location == RoomLocation.Create(query.Location));

            if (query.MinCapacity is not null)
            {
                var minCapacity = RoomCapacity.Create(query.MinCapacity.Value);
                rooms = rooms.Where(r => r.Capacity >= minCapacity);
            }

            if (query.IsActive is not null)
                rooms = rooms.Where(r => r.IsActive == query.IsActive);

            return await rooms.
                Select(r => new RoomReadModel
                {
                    Id = r.Id.Value,
                    Name = r.Name.Value,
                    Location = r.Location.Value,
                    Capacity = r.Capacity.Value,
                    IsActive = r.IsActive
                })
                .ToListAsync(ct);
        }

    }
}
