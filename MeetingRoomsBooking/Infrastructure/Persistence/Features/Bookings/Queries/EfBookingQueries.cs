using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Queries
{
    public class EfBookingQueries : IBookingQueries
    {
        private readonly BookingDbContext _db;

        public EfBookingQueries(BookingDbContext db) => _db = db;

        public async Task<bool> IsCanBooking(RoomId roomId, TimeSlot reqTime, CancellationToken ct)
        {
            return !await _db.BookingRequests
                .AnyAsync(b => b.RoomId == roomId &&
                b.Status == BookingStatus.Confirmed &&
                b.TimeSlot.StartAtUtc < reqTime.EndAtUtc &&
                b.TimeSlot.EndAtUtc > reqTime.StartAtUtc,
                ct);
        }


        public async Task<RoomReadModel?> GetById(
            RoomId roomId,
            RoomLocation location,
            CancellationToken ct)
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
