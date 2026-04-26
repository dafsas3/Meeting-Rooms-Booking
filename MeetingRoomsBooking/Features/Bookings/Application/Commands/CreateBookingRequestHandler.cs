using FluentValidation;
using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Abstractions.Persistence.UnitOfWork;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Application.Errors;
using MeetingRoomsBooking.Features.Bookings.Application.Extensions;
using MeetingRoomsBooking.Features.Bookings.Application.ReadModels;
using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.UserId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.ParticipantEmail;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.TimeSlot;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using MeetingRoomsBooking.Infrastructure.Persistence.DbExtensions;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands
{
    public class CreateBookingRequestHandler
    {
        private const int MaxHandleAttempts = 2;

        private readonly ICurrentUser _user;
        private readonly IBookingQueries _bookingQueries;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomQueries _roomQueries;
        private readonly ILogger<CreateBookingRequestHandler> _logger;
        private readonly IValidator<CreateBookingRequestCommand> _validator;
        private readonly IUnitOfWork _uow;

        public CreateBookingRequestHandler(
            ICurrentUser user,
            IBookingQueries queries,
            IBookingRepository repository,
            IRoomQueries roomQueries,
            ILogger<CreateBookingRequestHandler> logger,
            IValidator<CreateBookingRequestCommand> validator,
            IUnitOfWork uow)
        {
            _user = user;
            _bookingQueries = queries;
            _bookingRepository = repository;
            _uow = uow;
            _roomQueries = roomQueries;
            _logger = logger;
            _validator = validator;
        }


        public async Task<Result<CreateBookingRequestResponse>> Handle(
            CreateBookingRequestCommand cmd,
            Guid key,
            CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(cmd, ct);
            if (!validation.IsValid)
            {
                var firstError = validation.Errors.First();

                _logger.LogWarning("Create booking validation failed. " +
                    "UserId: {UserId}, RoomId: {RoomId}, Error: {Error}",
                    _user.EmployeeId,
                    cmd.RoomId,
                    firstError.ErrorMessage);

                return Result<CreateBookingRequestResponse>.BadRequest(
                    "VALIDATION_ERROR",
                    validation.Errors.First().ErrorMessage);
            }

            _logger.LogInformation(
                "Start creating booking. UserId: {UserId}, RoomId: {RoomId}, TimeSlot: {Start}-{End}, " +
                "IdempotencyKey: {Key}",
                _user.EmployeeId,
                cmd.RoomId,
                cmd.StartedAtUtc,
                cmd.EndAtUtc,
                key);

            if (_user.Role != UserRole.Employee)
            {
                _logger.LogWarning(
                    "Access denied. UserId: {UserId}, Role: {Role}",
                    _user.EmployeeId,
                    _user.Role);

                var error = BookingError.AccessDenied(_user.Role.ToString());
                return Result<CreateBookingRequestResponse>.Forbidden(error.Code, error.Message);
            }

            var preparedCtx = Prepare(cmd);

            for (int i = 1; i <= MaxHandleAttempts; i++)
            {
                _logger.LogDebug("Creating booking attempt. Attempt: {Attempt}, Key: {Key}", i, key);

                var idempotencyKey = IdempotencyKey.Create(key);

                var readEntity = await _bookingQueries.GetByIdempotencyKey(idempotencyKey, ct);

                if (readEntity is not null)
                {
                    _logger.LogInformation(
                        "Idempotent request hit. Returning existing booking. Id: {BookingId}",
                        readEntity.Id);

                    return Result<CreateBookingRequestResponse>.Ok(ToResponse(readEntity));
                }

                var existsRoom = await _roomQueries.GetById(preparedCtx.RoomId, ct);

                if (existsRoom is null)
                {
                    _logger.LogWarning(
                        "Room not found. RoomId: {RoomId}",
                        preparedCtx.RoomId.Value);

                    var error = BookingError.RoomNotFound(preparedCtx.RoomId.Value);
                    return Result<CreateBookingRequestResponse>.NotFound(error.Code, error.Message);
                }

                if (!existsRoom.IsActive)
                {
                    _logger.LogWarning(
                        "Room is not active. RoomId: {RoomId}",
                        preparedCtx.RoomId.Value);

                    return Result<CreateBookingRequestResponse>.Conflict(
                        BookingError.RoomNotActive.Code, BookingError.RoomNotActive.Message);
                }

                if (!await _bookingQueries.IsCanBooking(preparedCtx.RoomId, preparedCtx.TimeSlot, ct))
                {
                    _logger.LogInformation(
                        "Time slot conflict. RoomId: {RoomId}, Start: {Start}, End: {End}",
                        preparedCtx.RoomId.Value,
                        preparedCtx.TimeSlot.StartAtUtc,
                        preparedCtx.TimeSlot.EndAtUtc);

                    var error = BookingError.TimeSlotBusy(
                        preparedCtx.TimeSlot.StartAtUtc,
                        preparedCtx.TimeSlot.EndAtUtc);

                    return Result<CreateBookingRequestResponse>.BadRequest(error.Code, error.Message);
                }

                var entity = Create(preparedCtx, idempotencyKey);

                _logger.LogDebug(
                    "Creating booking entity. RoomId: {RoomId}, UserId: {UserId}",
                    preparedCtx.RoomId.Value, preparedCtx.EmployeeId.Value);

                _bookingRepository.Add(entity);

                try
                {
                    await _uow.SaveChangesAsync(ct);

                    _logger.LogInformation(
                        "Booking created successfully. BookingId: {BookingId}, RoomId: {RoomId}",
                        entity.Id.Value,
                        preparedCtx.RoomId.Value);

                    return Result<CreateBookingRequestResponse>.Created(ToResponse(
                        entity.ReadModelToCreateRequestResponse()));
                }

                catch (DbUpdateException ex)
                when (ex.TryGetUniqueConstraintName(out var keyName) &&
                keyName is DatabaseConstants.UniqueBookingIndexes.IdempotencyKey)
                {
                    _logger.LogWarning(ex,
                        "Idempotency conflict detected. Retry attempt {Attempt}, Key: {Key}", i, key);

                    _uow.ClearTracking();
                    continue;
                }
            }
            _logger.LogError(
                "Failed to create booking after retries. IdempotencyKey: {Key}", key);

            var conflict = BookingError.RequestAlreadyExists(key);
            return Result<CreateBookingRequestResponse>.Conflict(conflict.Code, conflict.Message);
        }


        private static BookingRequest Create(BookingDataPreparation context, IdempotencyKey key)
            => BookingRequest.Create(
                context.RoomId,
                context.EmployeeId,
                context.Role,
                context.TimeSlot,
                context.Purpose,
                context.Emails,
                key);


        private static CreateBookingRequestResponse ToResponse(BookingReadModel model)
            => new()
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


        private BookingDataPreparation Prepare(CreateBookingRequestCommand cmd)
        {
            var role = _user.Role.ToBookingActorRole();
            var roomId = RoomId.Create(cmd.RoomId);
            var employeeId = EmployeeId.Create(_user.EmployeeId);
            var timeSlot = TimeSlot.Create(cmd.StartedAtUtc, cmd.EndAtUtc);
            var purpose = MeetingPurpose.Create(cmd.Purpose);

            List<ParticipantEmail> participantEmails = new();

            foreach (var email in cmd.Emails)
            {
                participantEmails.Add(ParticipantEmail.Create(email));
            }

            return new(roomId, employeeId, timeSlot, purpose, role, participantEmails);
        }


        private sealed record BookingDataPreparation(
            RoomId RoomId,
            EmployeeId EmployeeId,
            TimeSlot TimeSlot,
            MeetingPurpose Purpose,
            BookingActorRole Role,
            List<ParticipantEmail> Emails);

    }
}
