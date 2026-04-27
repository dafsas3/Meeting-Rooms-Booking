using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Queries
{
    public class EfBookingQueries : IBookingQueries
    {
        private readonly BookingDbContext _db;

        public EfBookingQueries(BookingDbContext db) => _db = db;


        public async Task<bool> IsCanBookingAsync(RoomId roomId, TimeSlot reqTime, CancellationToken ct)
        {
            return !await _db.BookingRequests
                .AnyAsync(b => b.RoomId == roomId &&
                b.Status == BookingStatus.Confirmed &&
                b.TimeSlot.StartAtUtc < reqTime.EndAtUtc &&
                b.TimeSlot.EndAtUtc > reqTime.StartAtUtc,
                ct);
        }


        public async Task<BookingReadModel?> GetByIdempotencyKeyAsync(IdempotencyKey key, CancellationToken ct)
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


        public async Task<BookingReadModel?> GetByIdAsync(
            BookingRequestId id,
            CancellationToken ct)
            => 
            await _db.BookingRequests
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookingReadModel
            {
                Id = b.Id.Value,
                RoomId = b.RoomId.Value,
                EmployeeId = b.EmployeeId.Value,
                IdempotencyKey = b.IdempotencyKey.Value,
                StartedAtUtc = b.TimeSlot.StartAtUtc,
                EndAtUtc = b.TimeSlot.EndAtUtc,
                Purpose = b.MeetingPurpose.Value,
                Status = b.Status,
                Emails = b.ParticipantEmails
                .Select(e => e.Value)
                .ToList()
            })
            .FirstOrDefaultAsync(ct);

    }
}
