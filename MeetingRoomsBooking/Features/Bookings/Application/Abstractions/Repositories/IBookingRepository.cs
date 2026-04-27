using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;

namespace MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories
{
    public interface IBookingRepository
    {
        void Add(BookingRequest entity);
        Task<BookingRequest?> GetEntityByIdAsync(BookingRequestId id, CancellationToken ct);
    }
}
