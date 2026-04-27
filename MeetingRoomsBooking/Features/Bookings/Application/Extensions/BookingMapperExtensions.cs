using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Domain.Entities;

namespace MeetingRoomsBooking.Features.Bookings.Application.Extensions
{
    public static class BookingMapperExtensions
    {
        public static BookingReadModel EntityToReadModelResponse(
            this BookingRequest entity)
            => new BookingReadModel
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
