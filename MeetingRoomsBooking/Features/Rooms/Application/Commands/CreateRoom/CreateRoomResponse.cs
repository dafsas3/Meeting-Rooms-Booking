namespace MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom
{
    public sealed record CreateRoomResponse
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int ReqCapacity { get; init; }
        public required string Location { get; init; }
        public required bool IsActive { get; init; }
    }
}
