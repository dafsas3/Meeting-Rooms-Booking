using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;
using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
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

        Task<RoomReadModel?> GetReadByIdAsync(RoomId roomId, CancellationToken ct);

        Task<Room?> GetEntityByIdAsync(RoomId id, CancellationToken ct);

        /// <summary>
        /// Searches for records according to the specified parameters.
        /// </summary>
        /// 
        /// <param name="query">Query with parameters.</param>
        /// <param name="ct">Cancellation token.</param>
        /// 
        /// <returns>
        /// Filtered list of room entries.
        /// </returns>
        /// 
        /// <remarks>
        /// If no parameters are present, all records will be returned.
        /// </remarks>
        Task<List<RoomReadModel>> GetByFiltersAsync(
            GetRoomsQuery query, CancellationToken ct);
    }
}
