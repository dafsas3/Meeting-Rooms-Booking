using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;

namespace MeetingRoomsBooking.Features.Bookings.Application.Extensions
{
    public static class UserRoleExtensions
    {
        public static BookingActorRole ToBookingActorRole(this UserRole role) => role switch
        {
            UserRole.Employee => BookingActorRole.Employee,
            UserRole.Admin => BookingActorRole.Admin,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported user role.")
        };
    }
}
