using MeetingRoomsBooking.Features.Bookings.Domain.Entities;

namespace MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories
{
    public interface IBookingRepository
    {
        void Add(BookingRequest entity);
    }
}
