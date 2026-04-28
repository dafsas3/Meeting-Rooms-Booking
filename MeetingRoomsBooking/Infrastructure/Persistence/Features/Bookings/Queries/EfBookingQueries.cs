using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Queries.GetBookings;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
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
            return await _db.BookingRequests
                .AsNoTracking()
                .Where(b => b.IdempotencyKey == key)
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


        public async Task<List<BookingReadModel>> GetByFiltersAsync(
            GetBookingsQuery query, CancellationToken ct)
        {
            IQueryable<BookingRequest> bookings = _db.BookingRequests.AsNoTracking();

            if (query.RoomId is not null)
            {
                var roomId = RoomId.Create(query.RoomId.Value);
                bookings = bookings.Where(b => b.RoomId == roomId);
            }

            if (query.From is not null && query.To is not null)
            {
                var reqTime = TimeSlot.Create(query.From.Value, query.To.Value);

                bookings = bookings.Where(b =>
                b.TimeSlot.StartAtUtc < reqTime.EndAtUtc &&
                b.TimeSlot.EndAtUtc > reqTime.StartAtUtc);
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                var status = Enum.Parse<BookingStatus>(query.Status, ignoreCase: true);

                bookings = bookings.Where(b => b.Status == status);
            }

            return await bookings
                .OrderBy(b => b.TimeSlot.StartAtUtc)
                .Select(b => new BookingReadModel
                {
                    Id = b.Id.Value,
                    RoomId = b.RoomId.Value,
                    EmployeeId = b.EmployeeId.Value,
                    StartedAtUtc = b.TimeSlot.StartAtUtc,
                    EndAtUtc = b.TimeSlot.EndAtUtc,
                    Purpose = b.MeetingPurpose.Value,
                    Status = b.Status,
                    Emails = b.ParticipantEmails
                    .Select(e => e.Value)
                    .ToList()
                })
                .ToListAsync(ct);
        }


        public async Task<BookingDetailsReadModel?> GetDetailsByIdAsync(
            BookingRequestId id, CancellationToken ct)
        {
            return await _db.BookingRequests
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new BookingDetailsReadModel
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
                    .ToList(),

                    History = b.History
                    .OrderBy(h => h.CreatedAtUtc)
                    .Select(h => new BookingHistoryReadModel
                    {
                        Id = h.Id.Value,
                        EmployeeId = h.EmployeeId.Value,
                        FromStatus = h.FromStatus,
                        ToStatus = h.ToStatus,
                        ActorRole = h.ActorRole,
                        Reason = h.Reason.Value,
                        CreatedAtUtc = h.CreatedAtUtc
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync(ct);
        }


    }
}
