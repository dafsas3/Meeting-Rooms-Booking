namespace MeetingRoomsBooking.Features.Rooms.Application.ReadModels
{
    public sealed class RoomReadModel
    {
        public required int Id;
        public required string Name;
        public required int Capacity;
        public required string Location;
        public required bool IsActive;
    }
}
