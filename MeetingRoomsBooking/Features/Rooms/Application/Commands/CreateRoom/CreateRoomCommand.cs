namespace MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom
{
    public record CreateRoomCommand(
        string Name,
        int ReqCapacity,
        string Location,
        bool IsActive = true);
}
