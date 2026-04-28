using FluentValidation;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;

namespace MeetingRoomsBooking.Features.Bookings.Application.Queries.GetBookings
{
    public sealed class GetBookingsHandler
    {
        private readonly IBookingQueries _bookings;
        private readonly ILogger<GetBookingsHandler> _logger;
        private readonly IValidator<GetBookingsQuery> _validator;

        public GetBookingsHandler(
            IBookingQueries bookings,
            ILogger<GetBookingsHandler> logger,
            IValidator<GetBookingsQuery> validator)
        {
            _bookings = bookings;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<BookingReadModel>>> Handle(
            GetBookingsQuery query, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(query, ct);

            if (!validation.IsValid)
            {
                _logger.LogWarning("Get bookings validation failed. Error: {Error}",
                    validation.Errors.First().ErrorMessage);

                return Result<List<BookingReadModel>>.BadRequest(
                    "VALIDATION_ERROR",
                    validation.Errors.First().ErrorMessage);
            }

            _logger.LogInformation(
                "Fetching bookings. RoomId: {RoomId}, From: {From}, To: {To}, Status: {Status}",
                query.RoomId,
                query.From,
                query.To,
                query.Status);

            var result = await _bookings.GetByFiltersAsync(query, ct);

            _logger.LogInformation(
                "Bookings fetched successfully. Count: {Count}",
                result.Count);

            return Result<List<BookingReadModel>>.Ok(result);
        }
    }
}
