namespace MeetingRoomsBooking.Features.Bookings.Domain.Ids.EmployeeId
{
    public readonly record struct EmployeeId
    {
        public Guid Value { get; }

        private EmployeeId(Guid value) => Value = value;

        public static EmployeeId Create(Guid value)
        {
            if (value == Guid.Empty)
                throw InvalidEmployeeIdException.MustBeNotEmpty(value);

            return new EmployeeId(value);
        }

        public static EmployeeId FromDatabase(Guid value) => new(value);
    }
}
