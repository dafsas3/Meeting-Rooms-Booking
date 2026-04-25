namespace MeetingRoomsBooking.Features.Abstractions.Security
{
    public interface ICurrentUser
    {
        Guid EmployeeId { get; }
        UserRole Role { get; }
    }
}
