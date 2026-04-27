using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Exceptions
{
    public sealed class InvalidBookingStatusTransitionException : DomainException
    {
        private InvalidBookingStatusTransitionException(string reason, Dictionary<string, object?> meta)
            : base(
                  "INVALID_BOOKING_STATUS_TRANSITION",
                  $"Status transfer not possible. Reason: {reason}.",
                  meta)
        { }


        public static InvalidBookingStatusTransitionException TransferIsNotPossible(
            string from, string to) => 
            new($"Attempt transition from: \"{from}\" to \"{to}\".", new Dictionary<string, object?>
            {
                ["statusFrom"] = from,
                ["statusTo"] = to
            });

        public static InvalidBookingStatusTransitionException RoleCannotChangeStatus(
            string role, string to) =>
            new($"This role ({role}) cannot change the status in: {to}.", new Dictionary<string, object?>
            {
                ["role"] = role,
                ["statusTo"] = to
            });
    }
}
