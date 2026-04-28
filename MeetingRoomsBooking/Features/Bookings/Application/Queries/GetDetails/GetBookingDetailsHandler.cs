using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Errors;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;

namespace MeetingRoomsBooking.Features.Bookings.Application.Queries.GetDetails
{
    public sealed class GetBookingDetailsHandler
    {
        private readonly IBookingQueries _bookings;
        private ILogger<GetBookingDetailsHandler> _logger;

        public GetBookingDetailsHandler(
            IBookingQueries bookingQueries,
            ILogger<GetBookingDetailsHandler> logger)
        {
            _bookings = bookingQueries;
            _logger = logger;
        }

        public async Task<Result<BookingDetailsReadModel>> Handle(
            GetBookingDetailsQuery query, CancellationToken ct)
        {
            var bookingId = BookingRequestId.Create(query.BookingRequestId);

            _logger.LogInformation(
                "Fetching booking details. BookingId: {BookingId}",
                bookingId.Value);

            var result = await _bookings.GetDetailsByIdAsync(bookingId, ct);

            if (result is null)
            {
                _logger.LogWarning(
                    "Booking not found while fetching details. BookingId: {BookingId}",
                    bookingId.Value);

                var error = BookingError.BookingNotFound(bookingId.Value);
                return Result<BookingDetailsReadModel>.NotFound(error.Code, error.Message);
            }

            _logger.LogInformation(
                "Booking details fetched successfully. BookingId: {BookingId}",
                bookingId.Value);

            return Result<BookingDetailsReadModel>.Ok(result);
        }
    }
}
