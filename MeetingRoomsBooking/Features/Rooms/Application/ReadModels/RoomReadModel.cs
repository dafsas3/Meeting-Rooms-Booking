namespace MeetingRoomsBooking.Features.Rooms.Application.ReadModels
{
    public sealed class RoomReadModel
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int Capacity { get; init; }
        public required string Location { get; init; }
        public required bool IsActive { get; init; }
    }
}