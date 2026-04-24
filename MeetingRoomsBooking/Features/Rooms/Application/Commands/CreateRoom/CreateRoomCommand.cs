namespace MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom
{
    public record CreateRoomCommand(
        string Name,
        int ReqParticipantCount,
        string Location,
        bool IsActive);
}
