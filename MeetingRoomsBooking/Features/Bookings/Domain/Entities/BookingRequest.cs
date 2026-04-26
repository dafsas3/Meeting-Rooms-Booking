using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.UserId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.ParticipantEmail;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Entities
{
    public class BookingRequest
    {
        public BookingRequestId Id { get; private set; }
        public RoomId RoomId { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public TimeSlot TimeSlot { get; private set; } = null!;
        public MeetingPurpose MeetingPurpose { get; private set; } = null!;
        public BookingStatus Status { get; private set; }

        public IReadOnlyCollection<ParticipantEmail> ParticipantEmails => _participants.AsReadOnly();
        private readonly List<ParticipantEmail> _participants = new();

        public IReadOnlyCollection<BookingHistory> History => _history.AsReadOnly();
        private readonly List<BookingHistory> _history = new();

        private BookingRequest() { }

        private BookingRequest(
            RoomId roomId,
            EmployeeId employeeId,
            TimeSlot timeSlot,
            MeetingPurpose meetingPurpose,
            BookingStatus status,
            List<ParticipantEmail> participants)
        {
            RoomId = roomId;
            EmployeeId = employeeId;
            TimeSlot = timeSlot;
            MeetingPurpose = meetingPurpose;
            Status = status;
            _participants = participants;
        }


        public static BookingRequest Create(
            RoomId roomId,
            EmployeeId employeeId,
            BookingActorRole role,
            StatusTransferReason reason,
            TimeSlot time,
            MeetingPurpose purpose,
            List<ParticipantEmail> emails)
        {
            var entity = new BookingRequest(
                roomId,
                employeeId,
                time,
                purpose,
                BookingStatus.Created,
                emails);

            entity.WriteHistory(role, BookingStatus.Draft, reason);

            return entity;
        }


        private void WriteHistory(
            BookingActorRole role,
            BookingStatus to,
            StatusTransferReason reason)
        {
            _history.Add(BookingHistory.Create(
                EmployeeId,
                role,
                Status,
                to,
                reason));
        }

    }
}
