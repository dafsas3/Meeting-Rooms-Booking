namespace MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName
{
    public sealed record RoomName
    {
        public string Value { get; }
        private const int MinLength = 5;
        private const int MaxLength = 30;

        private RoomName(string value) => Value = value;


        public static RoomName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw InvalidRoomNameException.MustBeNotEmpty(value);

            var normalized = value.Trim();

            if (normalized.Length < MinLength)
                throw InvalidRoomNameException.LengthMustBeGreaterThan(value, MinLength);

            if (normalized.Length > MaxLength)
                throw InvalidRoomNameException.LengthMustBeLessThan(value, MaxLength);

            return new RoomName(normalized);
        }

    }
}
