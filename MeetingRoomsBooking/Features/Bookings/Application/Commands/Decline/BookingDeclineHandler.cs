using FluentValidation;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Application.Errors;
using MeetingRoomsBooking.Features.Bookings.Application.Extensions;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Application.Services;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Decline
{
    public sealed class BookingDeclineHandler
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICurrentUser _user;
        private readonly IProtectionConccurency _protectionConccurency;
        private readonly ILogger<BookingDeclineHandler> _logger;
        private readonly IValidator<BookingDeclineCommand> _validator;

        public BookingDeclineHandler(
            IBookingRepository bookingRepository,
            ICurrentUser currentUser,
            IProtectionConccurency protectionConccurency,
            ILogger<BookingDeclineHandler> logger,
            IValidator<BookingDeclineCommand> validator)
        {
            _bookingRepository = bookingRepository;
            _user = currentUser;
            _protectionConccurency = protectionConccurency;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<BookingDeclineResponse>> Handle(
            BookingDeclineCommand cmd, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(cmd, ct);

            if (!validation.IsValid)
            {
                _logger.LogWarning(
                    "Booking decline validation failed. BookingId: {BookingId}, Error: {Error}",
                    cmd.BookingRequestId,
                    validation.Errors.First().ErrorMessage);

                return Result<BookingDeclineResponse>.BadRequest(
                    "VALIDATION_ERROR", validation.Errors.First().ErrorMessage);
            }

            _logger.LogInformation(
                "Start declining booking. BookingId: {BookingId}",
                cmd.BookingRequestId);

            var reason = StatusTransferReason.Create(cmd.Reason);
            var bookingId = BookingRequestId.Create(cmd.BookingRequestId);

            var existsBooking = await _bookingRepository.GetEntityByIdAsync(bookingId, ct);

            if (existsBooking is null)
            {
                _logger.LogWarning(
                    "Booking not found during decline. BookingId: {BookingId}",
                    bookingId.Value);

                var error = BookingError.BookingNotFound(bookingId.Value);
                return Result<BookingDeclineResponse>.NotFound(error.Code, error.Message);
            }

            var role = _user.Role.ToBookingActorRole();

            existsBooking.Decline(role, reason);

            _logger.LogInformation(
                "Attempting to decline booking. BookingId: {BookingId}, Role: {Role}",
                bookingId.Value,
                role.ToString());

            if (!await _protectionConccurency.TrySaveAsync(ct))
            {
                _logger.LogWarning(
                    "Concurrency conflict during booking decline. BookingId: {BookingId}",
                    bookingId.Value);

                var error = BookingError.BookingStatusAlreadyChange;
                return Result<BookingDeclineResponse>.Conflict(error.Code, error.Message);
            }

            else
            {
                _logger.LogInformation(
                    "Booking declined successfully. BookingId: {BookingId}",
                bookingId.Value);

                return Result<BookingDeclineResponse>.Ok(ToResponse(
                    existsBooking.EntityToReadModelResponse()));
            }
        }


        private static BookingDeclineResponse ToResponse(BookingReadModel model)
            => new BookingDeclineResponse
            {
                Id = model.Id,
                RoomId = model.RoomId,
                EmployeeId = model.EmployeeId,
                IdempotencyKey = model.IdempotencyKey,
                StartedAtUtc = model.StartedAtUtc,
                EndAtUtc = model.EndAtUtc,
                Purpose = model.Purpose,
                Status = model.Status,
                Emails = model.Emails
            };
    }
}
