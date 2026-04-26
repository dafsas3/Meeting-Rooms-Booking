namespace MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingHistoryId
{
    public sealed record BookingHistoryId
    {
        public int Value { get; }

        private BookingHistoryId(int value) => Value = value;

        public static BookingHistoryId Create(int value)
        {
            if (value <= 0)
                throw InvalidBookingHistoryIdException.MustBeGreaterThanZero(value);

            return new BookingHistoryId(value);
        }

        public static BookingHistoryId FromDatabase(int value) => new(value);
    }
}
