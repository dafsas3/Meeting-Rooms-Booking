using MeetingRoomsBooking.Features.Abstractions.Domain;

namespace MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot
{
    public sealed class InvalidTimeSlotException : DomainException
    {
        private InvalidTimeSlotException(string code, string message, Dictionary<string, object?> meta)
            : base(code, message, meta)
        {

        }


        public static InvalidTimeSlotException StartMustBeUtc(DateTime value) =>
            new("INVALID_TIMESLOT_START", "Time slot start must be in UTC.",
                new Dictionary<string, object?> { ["StartAt"] = value });

        public static InvalidTimeSlotException EndMustBeUtc(DateTime value) =>
            new("INVALID_TIMESLOT_END", "Time slot end must be in UTC.",
                new Dictionary<string, object?> { ["End"] = value });

        public static InvalidTimeSlotException EndMustBeGreaterThanStart(DateTime start, DateTime end)
            => new("INVALID_TIMESLOT_RANGE", "Time slot end must be greater than start.",
                new Dictionary<string, object?> { ["StartAt"] = start, ["EndAt"] = end });

        public static InvalidTimeSlotException DurationMustBeLess(
            DateTime start,
            DateTime end,
            TimeSpan maxDuration) =>
            new("INVALID_TIMESLOT_DURATION",
                $"Time slot duration must be less than or equal to: {maxDuration.TotalHours}.",
                new Dictionary<string, object?>
                {
                    ["StartAt"] = start,
                    ["EndAt"] = end,
                    ["MaxDuration"] = maxDuration.TotalHours
                });
    }
}
