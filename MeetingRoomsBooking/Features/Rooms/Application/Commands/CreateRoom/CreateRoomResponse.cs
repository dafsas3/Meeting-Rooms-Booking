namespace MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom
{
    public sealed record CreateRoomResponse
    {
        public required int Id;
        public required string Name;
        public required int ReqCapacity;
        public required string Location;
        public required bool IsActive;
    }
}
