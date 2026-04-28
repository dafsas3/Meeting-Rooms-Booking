using FluentValidation;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Application.Errors;
using MeetingRoomsBooking.Features.Bookings.Application.Extensions;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Application.Services;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.EmployeeId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Cancel
{
    public sealed class BookingCancelHandler
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICurrentUser _user;
        private readonly IProtectionConccurency _protectionConccurency;
        private readonly ILogger<BookingCancelHandler> _logger;
        private readonly IValidator<BookingCancelCommand> _validator;

        public BookingCancelHandler(
            IBookingRepository bookingRepository,
            ICurrentUser currentUser,
            IProtectionConccurency protectionConccurency,
            ILogger<BookingCancelHandler> logger,
            IValidator<BookingCancelCommand> validator)
        {
            _bookingRepository = bookingRepository;
            _user = currentUser;
            _protectionConccurency = protectionConccurency;
            _logger = logger;
            _validator = validator;
        }


        public async Task<Result<BookingCancelResponse>> Handle(
            BookingCancelCommand cmd, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(cmd, ct);

            if (!validation.IsValid)
            {
                _logger.LogWarning(
                    "Booking cancel validation failed. BookingId: {BookingId}, Error: {Error}",
                    cmd.BookingRequestId,
                    validation.Errors.First().ErrorMessage);

                return Result<BookingCancelResponse>.BadRequest(
                    "VALIDATION_ERROR", validation.Errors.First().ErrorMessage);
            }

            _logger.LogInformation(
                "Start cancelling booking. BookingId: {BookingId}",
                cmd.BookingRequestId);

            var bookingId = BookingRequestId.Create(cmd.BookingRequestId);
            var reason = StatusTransferReason.Create(cmd.Reason);

            var existsBooking = await _bookingRepository.GetEntityByIdAsync(bookingId, ct);

            if (existsBooking is null)
            {
                _logger.LogWarning(
                    "Booking not found during cancel. BookingId: {BookingId}",
                    bookingId.Value);

                var error = BookingError.BookingNotFound(bookingId.Value);
                return Result<BookingCancelResponse>.NotFound(error.Code, error.Message);
            }

            var role = _user.Role.ToBookingActorRole();
            var employeeId = EmployeeId.Create(_user.EmployeeId);            

            existsBooking.Cancel(role, employeeId, reason);

            _logger.LogInformation(
                "Attempting to cancel booking. BookingId: {BookingId}, EmployeeId: {EmployeeId}",
                bookingId.Value,
                existsBooking.EmployeeId.Value);

            if (!await _protectionConccurency.TrySaveAsync(ct))
            {
                _logger.LogWarning(
                    "Concurrency conflict during booking cancel. BookingId: {BookingId}",
                    bookingId.Value);

                var error = BookingError.BookingStatusAlreadyChange;
                return Result<BookingCancelResponse>.Conflict(error.Code, error.Message);
            }

            else
            {
                _logger.LogInformation(
                    "Booking cancelled successfully. BookingId: {BookingId}, EmployeeId: {EmployeeId}",
                    bookingId.Value,
                    existsBooking.EmployeeId.Value);

                return Result<BookingCancelResponse>.Ok(ToResponse(
                    existsBooking.EntityToReadModelResponse()));
            }
        }


        private static BookingCancelResponse ToResponse(BookingReadModel model)
            => new BookingCancelResponse
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
