namespace MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId
{
    public readonly record struct BookingRequestId
    {
        public int Value { get; }

        private BookingRequestId(int value) => Value = value;

        public static BookingRequestId Create(int value)
        {
            if (value <= 0)
                throw InvalidBookingRequestIdException.MustBeGreaterThanZero(value);

            return new BookingRequestId(value);
        }


        public static BookingRequestId FromDatabase(int value) =>
            new(value);
    }
}
