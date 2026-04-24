namespace MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity
{
    public sealed record RoomCapacity
    {
        public int Value { get; }
        private const int MaxCapacity = 20;

        private RoomCapacity(int value) => Value = value;

        public static RoomCapacity Create(int value)
        {
            if (value <= 0)
                throw InvalidRoomCapacityException.MustBeGreaterThanZero(value);

            if (value > MaxCapacity)
                throw InvalidRoomCapacityException.CapacityMustBeLessThan(value, MaxCapacity);

            return new RoomCapacity(value);
        }
        
    }
}
