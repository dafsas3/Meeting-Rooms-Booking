using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;

namespace MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries
{
    public interface IBookingQueries
    {
        /// <summary>
        /// Checks if there is a time slot conflict. to create a reservation for the specified time.
        /// </summary>
        /// 
        /// <param name="roomId">Room ID value object.</param>
        /// <param name="reqTime">Required time interval value object.</param>
        /// <param name="ct">Cancellation token.</param>
        /// 
        /// <returns>
        /// True if there is no conflict and you can book.
        /// </returns>
        /// 
        /// <remarks>
        /// Used when attempting to request a new reservation for a specified time interval.
        /// </remarks>
        Task<bool> IsCanBookingAsync(RoomId roomId, TimeSlot reqTime, CancellationToken ct);


        /// <summary>
        /// Trying to get an existing booking.
        /// </summary>
        /// 
        /// <param name="key">Idempotency key value object.</param>
        /// <param name="ct">Cancellation token.</param>
        /// 
        /// <returns>
        /// Read model with booking data if it exists.
        /// </returns>
        /// 
        /// <remarks>
        /// Used to prevent an error due to a double request.
        /// </remarks>
        Task<BookingReadModel?> GetByIdempotencyKeyAsync(IdempotencyKey key, CancellationToken ct);

        Task<BookingReadModel?> GetByIdAsync(BookingRequestId id, CancellationToken ct);
    }
}
