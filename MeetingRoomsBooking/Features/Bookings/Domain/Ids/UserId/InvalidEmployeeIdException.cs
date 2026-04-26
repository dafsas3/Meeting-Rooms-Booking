using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Ids.UserId
{
    public sealed class InvalidEmployeeIdException : DomainException
    {
        private InvalidEmployeeIdException(Guid value, string reason) : base(
            code: "INVALID_EMPLOYEE_ID", message: $"Incorrect employee ID: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidEmployeeIdException MustBeNotEmpty(Guid value) =>
            new(value, "The employee ID cannot be empty.");
    }
}
