using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Exceptions;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.EmployeeId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.ParticipantEmail;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;
using MeetingRoomsBooking.Features.Bookings.Domain.Workflow;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Entities
{
    public class BookingRequest
    {
        public BookingRequestId Id { get; private set; }
        public RoomId RoomId { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public IdempotencyKey IdempotencyKey { get; private set; } = null!;
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
            List<ParticipantEmail> participants,
            IdempotencyKey key)
        {
            RoomId = roomId;
            EmployeeId = employeeId;
            TimeSlot = timeSlot;
            MeetingPurpose = meetingPurpose;
            Status = status;
            _participants = participants;
            IdempotencyKey = key;
        }


        public static BookingRequest Create(
            RoomId roomId,
            EmployeeId employeeId,
            BookingActorRole role,
            TimeSlot time,
            MeetingPurpose purpose,
            List<ParticipantEmail> emails,
            IdempotencyKey key)
        {
            var to = BookingStatus.Draft;

            RoleCanChangeStatus(role, to);

            var entity = new BookingRequest(
                roomId,
                employeeId,
                time,
                purpose,
                to,
                emails,
                key);

            entity.WriteHistory(
                role,
                to,
                StatusTransferReason.Create("The reservation has been saved in draft."));

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


        public void Submit(BookingActorRole role, EmployeeId employeeId)
        {
            var statusTo = BookingStatus.Submitted;

            IsOwner(employeeId);
            RoleCanChangeStatus(role, statusTo);

            ChangeStatus(
                role,
                statusTo,
                StatusTransferReason.Create("Booking submitted."));
        }


        public void Confirm(BookingActorRole role)
        {
            var statusTo = BookingStatus.Confirmed;
            RoleCanChangeStatus(role, statusTo);

            ChangeStatus(
                role,
                statusTo,
                StatusTransferReason.Create("Confirmed by admin."));
        }


        public void Decline(BookingActorRole role, StatusTransferReason reason)
        {
            var statusTo = BookingStatus.Declined;
            RoleCanChangeStatus(role, statusTo);

            ChangeStatus(role, statusTo, reason);
        }


        public void Cancel(
            BookingActorRole role,
            EmployeeId employeeId,
            StatusTransferReason reason)
        {
            var statusTo = BookingStatus.Cancelled;

            IsOwner(employeeId);
            RoleCanChangeStatus(role, statusTo);

            ChangeStatus(role, statusTo, reason);
        }


        private void ChangeStatus(
            BookingActorRole role,
            BookingStatus to,
            StatusTransferReason reason)
        {
            if (!BookingWorkflow.IsCanTransitionStatus(Status, to))
            {
                throw InvalidBookingStatusTransitionException.TransferIsNotPossible(
                    Status.ToString(), to.ToString());
            }

            WriteHistory(role, to, reason);
            Status = to;
        }


        private static void RoleCanChangeStatus(BookingActorRole role, BookingStatus to)
        {
            if (!BookingWorkflow.IsRoleCanTransition(role, to))
            {
                throw InvalidBookingStatusTransitionException.RoleCannotChangeStatus(
                    role.ToString(), to.ToString());
            }
        }


        private void IsOwner(EmployeeId employeeId)
        {
            if (EmployeeId != employeeId)
                throw InvalidEmployeeIdException.Mismatch(employeeId.Value);
        }


        public int GetParticipantsCount() => _participants.Count;

    }
}
