namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot
{
    public sealed record TimeSlot
    {
        public DateTime StartAtUtc { get; }
        public DateTime EndAtUtc { get; }

        private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

        private TimeSlot() { }

        private TimeSlot(DateTime startAtUtc, DateTime endAtUtc)
        {
            StartAtUtc = startAtUtc;
            EndAtUtc = endAtUtc;
        }


        public static TimeSlot Create(DateTime startAtUtc, DateTime endAtUtc)
        {
            if (startAtUtc.Kind != DateTimeKind.Utc)
                throw InvalidTimeSlotException.StartMustBeUtc(startAtUtc);

            if (endAtUtc.Kind != DateTimeKind.Utc)
                throw InvalidTimeSlotException.EndMustBeUtc(endAtUtc);

            if (endAtUtc <= startAtUtc)
                throw InvalidTimeSlotException.EndMustBeGreaterThanStart(startAtUtc, endAtUtc);

            if (endAtUtc - startAtUtc > MaxDuration)
                throw InvalidTimeSlotException.DurationMustBeLess(startAtUtc, endAtUtc, MaxDuration);

            return new TimeSlot(startAtUtc, endAtUtc);
        }


        public bool Overlaps(TimeSlot other) =>
            StartAtUtc < other.EndAtUtc && EndAtUtc > other.StartAtUtc;

    }
}
