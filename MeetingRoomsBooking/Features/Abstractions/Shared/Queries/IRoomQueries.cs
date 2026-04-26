using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;

namespace MeetingRoomsBooking.Features.Abstractions.Shared.Queries
{
    public interface IRoomQueries
    {

        /// <summary>
        /// Checks if a room with the given name exists at the given location.
        /// </summary>
        /// 
        /// <param name="name"> Room name value object.</param>
        /// <param name="location">Room location value object.</param>
        /// <param name="ct">Cancellation token.</param>
        /// 
        /// <returns>
        /// True if a room with the same name exists in the given location.
        /// </returns>
        /// 
        /// <remarks>
        /// Used during room creation to prevent duplicates.
        /// </remarks>
        Task<bool> IsExistsByNameAndLocationAsync(
            RoomName name,
            RoomLocation location,
            CancellationToken ct);

        Task<RoomReadModel?> GetById(RoomId roomId, CancellationToken ct);
    }
}
