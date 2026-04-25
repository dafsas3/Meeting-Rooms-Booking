namespace MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId
{
    public readonly record struct RoomId
    {
        public int Value { get; }

        private RoomId(int value) => Value = value;

        public static RoomId Create(int value)
        {
            if (value <= 0)
                throw InvalidRoomIdException.MustBeGreaterThanZero(value);

            return new RoomId(value);
        }

        public static RoomId FromDatabase(int value) => new(value);
    }
}
