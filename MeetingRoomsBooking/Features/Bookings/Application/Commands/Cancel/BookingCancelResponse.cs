using MeetingRoomsBooking.Features.Bookings.Domain.Enums;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Cancel
{
    public sealed class BookingCancelResponse
    {
        public required int Id { get; init; }
        public required int RoomId { get; init; }
        public required Guid EmployeeId { get; init; }
        public required Guid IdempotencyKey { get; init; }
        public required DateTime StartedAtUtc { get; init; }
        public required DateTime EndAtUtc { get; init; }
        public required string Purpose { get; init; }
        public required BookingStatus Status { get; init; }
        public required List<string> Emails { get; init; } = new();
    }
}
