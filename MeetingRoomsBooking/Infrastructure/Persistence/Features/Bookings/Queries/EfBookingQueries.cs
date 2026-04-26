using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
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





        public async Task<BookingReadModel?> GetByIdempotencyKey(IdempotencyKey key, CancellationToken ct)
        {
            var entity = await _db.BookingRequests
                .Include("_participants")
                .FirstOrDefaultAsync(b => b.IdempotencyKey == key, ct);

            if (entity is null) return null;

            return new BookingReadModel
            {
                Id = entity.Id.Value,
                RoomId = entity.RoomId.Value,
                EmployeeId = entity.EmployeeId.Value,
                IdempotencyKey = entity.IdempotencyKey.Value,
                StartedAtUtc = entity.TimeSlot.StartAtUtc,
                EndAtUtc = entity.TimeSlot.EndAtUtc,
                Purpose = entity.MeetingPurpose.Value,
                Status = entity.Status,
                Emails = entity.ParticipantEmails
                    .Select(e => e.Value)
                    .ToList()
            };
        }
    }
}
