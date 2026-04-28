using MeetingRoomsBooking.Features.Bookings.Domain.Enums;

namespace MeetingRoomsBooking.Features.Bookings.Application.ReadModels
{
    public sealed class BookingHistoryReadModel
    {
        public required int Id { get; set; }
        public required Guid EmployeeId { get; set; }
        public required BookingActorRole ActorRole { get; set; }
        public required BookingStatus FromStatus { get; set; }
        public required BookingStatus ToStatus { get; set; }
        public required string Reason { get; set; }
        public required DateTime CreatedAtUtc { get; set; }
    }
}
