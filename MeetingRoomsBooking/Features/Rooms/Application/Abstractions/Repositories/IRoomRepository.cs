using MeetingRoomsBooking.Features.Rooms.Domain.Entities;

namespace MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Repositories
{
    public interface IRoomRepository
    {
        void Add(Room entity);
    }
}
