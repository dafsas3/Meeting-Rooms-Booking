using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;

namespace MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Queries
{
    public interface IRoomQueries
    {
        Task<bool> IsExistsByNameAsync(RoomName name, CancellationToken ct);
    }
}
