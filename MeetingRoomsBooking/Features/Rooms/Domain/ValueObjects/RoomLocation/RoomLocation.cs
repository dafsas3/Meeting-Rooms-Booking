namespace MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation
{
    public sealed record RoomLocation
    {
        public string Value { get; }
        private const int MinLength = 4;
        private const int MaxLength = 30;
        
        private RoomLocation(string value) => Value = value;

        public static RoomLocation Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw InvalidRoomLocationException.MustBeNotEmpty(value);

            var normalized = value.Trim();

            if (normalized.Length < MinLength)
                throw InvalidRoomLocationException.LengthMustBeGreaterThan(value, MinLength);

            if (normalized.Length > MaxLength)
                throw InvalidRoomLocationException.LengthMustBeLessThan(value, MaxLength);

            return new RoomLocation(normalized);
        }
    }
}
