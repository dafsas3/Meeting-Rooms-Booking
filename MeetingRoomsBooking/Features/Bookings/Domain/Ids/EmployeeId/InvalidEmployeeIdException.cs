using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.Ids.EmployeeId
{
    public sealed class InvalidEmployeeIdException : DomainException
    {
        private InvalidEmployeeIdException(Guid value, string type, string reason) : base(
            code: type, message: $"Incorrect employee ID: \"{value}\". Reason: {reason}.",
            meta: new Dictionary<string, object?>
            {
                ["Value"] = value,
                ["Reason"] = reason
            })
        { }


        public static InvalidEmployeeIdException MustBeNotEmpty(Guid value) =>
            new(value, "INVALID_EMPLOYEE_ID", "The employee ID cannot be empty.");

        public static InvalidEmployeeIdException Mismatch(Guid value) =>
            new(value, "INVALID_MISMATCH_EMPLOYEE_ID",
                "The employee ID does not match the booking owner.");
    }
}
