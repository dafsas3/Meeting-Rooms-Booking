using MeetingRoomsBooking.Features.Bookings.Domain.Enums;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Workflow
{
    public static class BookingWorkflow
    {
        private static readonly Dictionary<BookingStatus, HashSet<BookingStatus>>
            AllowedTransition = new()
            {
                [BookingStatus.Draft] = [BookingStatus.Submitted],
                [BookingStatus.Submitted] = [BookingStatus.Confirmed, BookingStatus.Declined],
                [BookingStatus.Confirmed] = [BookingStatus.Cancelled]
            };


        private static readonly Dictionary<BookingActorRole, HashSet<BookingStatus>> AllowedRoles
            = new()
            {
                [BookingActorRole.Employee] =
                [
                    BookingStatus.Draft,
                    BookingStatus.Submitted,
                    BookingStatus.Cancelled
                ],

                [BookingActorRole.Admin] = [BookingStatus.Confirmed, BookingStatus.Declined]
            };


        public static bool IsCanTransitionStatus(BookingStatus from, BookingStatus to)
            =>
            AllowedTransition.TryGetValue(from, out var next) && next.Contains(to);


        public static bool IsRoleCanTransition(BookingActorRole role, BookingStatus to)
            =>
            AllowedRoles.TryGetValue(role, out var next) && next.Contains(to);
    }
}
