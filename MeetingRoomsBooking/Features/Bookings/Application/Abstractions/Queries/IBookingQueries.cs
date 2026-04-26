using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;

namespace MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries
{
    public interface IBookingQueries
    {
        Task<bool> IsCanBooking(RoomId roomId, TimeSlot reqTime, CancellationToken ct);

        Task<RoomReadModel?> GetById(
            RoomId roomId,
            RoomLocation location,
            CancellationToken ct);
    }
}
