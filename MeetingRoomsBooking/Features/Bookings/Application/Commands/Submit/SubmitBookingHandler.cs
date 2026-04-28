using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Application.Errors;
using MeetingRoomsBooking.Features.Bookings.Application.Extensions;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Application.Services;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.EmployeeId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Submit
{
    public sealed class SubmitBookingHandler
    {
        private readonly IBookingQueries _bookingQueries;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomQueries _roomQueries;
        private readonly ILogger<SubmitBookingHandler> _logger;
        private readonly ICurrentUser _user;
        private readonly IProtectionConccurency _protectionConccurency;

        public SubmitBookingHandler(
            IBookingQueries bookingQueries,
            IBookingRepository bookingRepository,
            IRoomQueries roomQueriest,
            ILogger<SubmitBookingHandler> logger,
            ICurrentUser user,
            IProtectionConccurency protectionConccurency)
        {
            _bookingQueries = bookingQueries;
            _roomQueries = roomQueriest;
            _bookingRepository = bookingRepository;
            _logger = logger;
            _user = user;
            _protectionConccurency = protectionConccurency;
        }


        public async Task<Result<SubmitBookingResponse>> Handle(
            SubmitBookingCommand cmd, CancellationToken ct)
        {
            _logger.LogInformation("The creation of the transition request has begun. " +
                "Booking request ID: {BookingId}.", cmd.BookingRequestId);

            var bookingId = BookingRequestId.Create(cmd.BookingRequestId);

            var existsBooking = await _bookingRepository.GetEntityByIdAsync(bookingId, ct);

            if (existsBooking is null)
            {
                _logger.LogWarning("Unable to find booking by ID : {BookingId}.",
                    bookingId.Value);

                var error = BookingError.BookingNotFound(bookingId.Value);
                return Result<SubmitBookingResponse>.NotFound(error.Code, error.Message);
            }

            var role = _user.Role.ToBookingActorRole();
            var employeeId = EmployeeId.Create(_user.EmployeeId);

            var roomId = RoomId.Create(existsBooking.RoomId.Value);

            var existsRoom = await _roomQueries.GetEntityByIdAsync(roomId, ct);

            if (existsRoom is null)
            {
                _logger.LogWarning("Room not found by ID: {RoomId}.",
                    roomId.Value);

                var error = BookingError.RoomNotFound(roomId.Value);
                return Result<SubmitBookingResponse>.NotFound(error.Code, error.Message);
            }

            if (!existsRoom.CanFit(existsBooking.GetParticipantsCount()))
            {
                _logger.LogWarning("The reservation exceeds the maximum " +
                    "number of participants in the room. Booking participants count: {Count}",
                    existsBooking.GetParticipantsCount());

                var error = BookingError.ManyParticipants;
                return Result<SubmitBookingResponse>.BadRequest(error.Code, error.Message);
            }

            if (!existsRoom.IsActive)
            {
                _logger.LogWarning("The room is not active. ID: {RoomId}.",
                    roomId.Value);

                var error = BookingError.RoomNotActive;
                return Result<SubmitBookingResponse>.Conflict(error.Code, error.Message);
            }

            var reqTimeSlot = TimeSlot.Create(
                existsBooking.TimeSlot.StartAtUtc, existsBooking.TimeSlot.EndAtUtc);

            if (!await _bookingQueries.IsCanBookingAsync(roomId, reqTimeSlot, ct))
            {
                _logger.LogWarning("Time interval overlap detected. " +
                    "RoomID: {RoomId}, StartAtUtc: {Start}, EndAtUtc: {End}.",
                    roomId.Value,
                    reqTimeSlot.StartAtUtc,
                    reqTimeSlot.EndAtUtc);

                var error = BookingError.TimeSlotBusy(reqTimeSlot.StartAtUtc, reqTimeSlot.EndAtUtc);
                return Result<SubmitBookingResponse>.Conflict(error.Code, error.Message);
            }

            existsBooking.Submit(role, employeeId);

            _logger.LogInformation("Attempting to change the booking status to submitted. " +
                "Booking request ID: {BookingId}, RoomID: {RoomId}, Role: {Role}, StartAtUtc: {Start}, " +
                "EndATUtc: {End}.",
                existsBooking.Id.Value,
                roomId.Value,
                role.ToString(),
                reqTimeSlot.StartAtUtc,
                reqTimeSlot.EndAtUtc);

            if (!await _protectionConccurency.TrySaveAsync(ct))
            {
                _logger.LogWarning("A concurrency error was detected. " +
                    "The booking status has already been changed. " +
                    "Booking request ID: {BookingId}, RoomID: {RoomId}",
                    cmd.BookingRequestId,
                    roomId.Value);

                var error = BookingError.BookingStatusAlreadyChange;
                return Result<SubmitBookingResponse>.Conflict(error.Code, error.Message);
            }

            else
            {
                _logger.LogInformation("Saving the new status is successful. " +
                    "Booking request ID: {BookingId} Status: {Status}.",
                    existsBooking.Id.Value,
                    existsBooking.Status.ToString());

                return Result<SubmitBookingResponse>.Ok(ToResponse(
                    existsBooking.EntityToReadModelResponse()));
            }
        }


        private static SubmitBookingResponse ToResponse(BookingReadModel model)
            => new SubmitBookingResponse
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
