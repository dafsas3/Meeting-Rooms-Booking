namespace MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms
{
    public sealed record GetRoomsQuery(
        string? Name,
        string? Location,
        int? MinCapacity,
        bool? IsActive);
}
