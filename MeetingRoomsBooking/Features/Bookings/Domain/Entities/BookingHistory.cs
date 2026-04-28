using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingHistoryId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.EmployeeId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Entities
{
    public sealed class BookingHistory
    {
        public BookingHistoryId Id { get; private set; }
        public BookingRequestId BookingRequestId { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public BookingActorRole ActorRole { get; private set; }
        public BookingStatus FromStatus { get; private set; }
        public BookingStatus ToStatus { get; private set; }
        public StatusTransferReason Reason { get; private set; } = null!;
        public DateTime CreatedAtUtc { get; private set; }

        private BookingHistory() { }

        private BookingHistory(
            EmployeeId employeeId,
            BookingActorRole role,
            BookingStatus from,
            BookingStatus to,
            StatusTransferReason reason)
        {
            EmployeeId = employeeId;
            ActorRole = role;
            FromStatus = from;
            ToStatus = to;
            Reason = reason;
            CreatedAtUtc = DateTime.UtcNow;
        }


        public static BookingHistory Create(
            EmployeeId employeeId,
            BookingActorRole role,
            BookingStatus from,
            BookingStatus to,
            StatusTransferReason reason)
        {
            return new BookingHistory(
                employeeId,
                role,
                from,
                to,
                reason);
        }
    }
}
