using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Submit;
using MeetingRoomsBooking.Features.Bookings.Application.Errors;
using MeetingRoomsBooking.Features.Bookings.Application.Extensions;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Application.Services;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Confirm
{
    public sealed class ConfirmBookingHandler
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingQueries _bookingQueries;
        private readonly IRoomQueries _roomQueries;
        private readonly ICurrentUser _user;
        private readonly ILogger<ConfirmBookingHandler> _logger;
        private readonly IProtectionConccurency _protectionConccurency;

        public ConfirmBookingHandler(
            IBookingRepository repo,
            IBookingQueries bookingQueries,
            IRoomQueries roomQueries,
            ICurrentUser user,
            ILogger<ConfirmBookingHandler> logger,
            IProtectionConccurency conccurency)
        {
            _bookingRepository = repo;
            _bookingQueries = bookingQueries;
            _roomQueries = roomQueries;
            _user = user;
            _logger = logger;
            _protectionConccurency = conccurency;
        }


        public async Task<Result<ConfirmBookingResponse>> Handle(
            ConfirmBookingCommand cmd, CancellationToken ct)
        {
            var bookingId = BookingRequestId.Create(cmd.Id);

            _logger.LogInformation(
                "Start confirming booking. BookingId: {BookingId}",
                bookingId.Value);

            var existsBooking = await _bookingRepository.GetEntityByIdAsync(bookingId, ct);

            if (existsBooking is null)
            {
                _logger.LogWarning(
                    "Booking not found. BookingId: {BookingId}",
                    bookingId.Value);

                var error = BookingError.BookingNotFound(bookingId.Value);
                return Result<ConfirmBookingResponse>.NotFound(error.Code, error.Message);
            }

            var role = _user.Role.ToBookingActorRole();

            var roomId = RoomId.Create(existsBooking.RoomId.Value);

            var existsRoom = await _roomQueries.GetEntityByIdAsync(roomId, ct);

            if (existsRoom is null)
            {
                _logger.LogWarning(
                    "Room not found. RoomId: {RoomId}, BookingId: {BookingId}",
                    roomId.Value, bookingId.Value);

                var error = BookingError.RoomNotFound(roomId.Value);
                return Result<ConfirmBookingResponse>.NotFound(error.Code, error.Message);
            }

            if (!existsRoom.CanFit(existsBooking.GetParticipantsCount()))
            {
                _logger.LogWarning("The reservation exceeds the maximum " +
                    "number of participants in the room. Booking participants count: {Count}",
                    existsBooking.GetParticipantsCount());

                var error = BookingError.ManyParticipants;
                return Result<ConfirmBookingResponse>.Conflict(error.Code, error.Message);
            }

            if (!existsRoom.IsActive)
            {
                _logger.LogWarning(
                    "Room is inactive. RoomId: {RoomId}, BookingId: {BookingId}",
                    roomId.Value, bookingId.Value);

                var error = BookingError.RoomNotActive;
                return Result<ConfirmBookingResponse>.NotFound(error.Code, error.Message);
            }

            var reqTimeSlot = TimeSlot.Create(
                existsBooking.TimeSlot.StartAtUtc, existsBooking.TimeSlot.EndAtUtc);

            if (!await _bookingQueries.IsCanBookingAsync(roomId, reqTimeSlot, ct))
            {
                _logger.LogWarning(
                    "Time slot conflict. RoomId: {RoomId}, Start: {Start}, End: {End}, BookingId: {BookingId}",
                    roomId.Value,
                    reqTimeSlot.StartAtUtc,
                    reqTimeSlot.EndAtUtc,
                    bookingId.Value);

                var error = BookingError.TimeSlotBusy(reqTimeSlot.StartAtUtc, reqTimeSlot.EndAtUtc);
                return Result<ConfirmBookingResponse>.Conflict(error.Code, error.Message);
            }

            existsBooking.Confirm(role);

            _logger.LogInformation(
                "Attempting to confirm booking. BookingId: {BookingId}, RoomId: {RoomId}, Role: {Role}",
                bookingId.Value,
                roomId.Value,
                role.ToString());

            if (!await _protectionConccurency.TrySaveAsync(ct))
            {
                _logger.LogWarning(
                    "Concurrency conflict during confirm. BookingId: {BookingId}, RoomId: {RoomId}",
                    bookingId.Value,
                    roomId.Value);

                var error = BookingError.BookingStatusAlreadyChange;
                return Result<ConfirmBookingResponse>.Conflict(error.Code, error.Message);
            }

            else
            {
                _logger.LogInformation(
                    "Booking confirmed successfully. BookingId: {BookingId}, RoomId: {RoomId}",
                    bookingId.Value,
                    roomId.Value);

                return Result<ConfirmBookingResponse>.Ok(ToResponse(
                existsBooking.EntityToReadModelResponse()));
            }
        }


        private static ConfirmBookingResponse ToResponse(BookingReadModel model)
            => new ConfirmBookingResponse
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
