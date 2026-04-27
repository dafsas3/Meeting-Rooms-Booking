using MeetingRoomsBooking.Features.Bookings.Domain.Enums;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.CreateBookingRequest
{
    public sealed record CreateBookingRequestResponse
    {
        public required int Id { get; set; }
        public required int RoomId { get; set; }
        public required Guid EmployeeId { get; set; }
        public required Guid IdempotencyKey { get; set; }
        public required DateTime StartedAtUtc { get; set; }
        public required DateTime EndAtUtc { get; set; }
        public required string Purpose { get; set; }
        public required BookingStatus Status { get; set; }
        public required List<string> Emails { get; set; } = new();
    }
}
