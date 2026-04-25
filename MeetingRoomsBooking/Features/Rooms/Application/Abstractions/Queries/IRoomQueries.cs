using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;

namespace MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Queries
{
    public interface IRoomQueries
    {
        Task<bool> IsExistsByNameAndLocationAsync(
            RoomName name,
            RoomLocation location,
            CancellationToken ct);
    }
}
