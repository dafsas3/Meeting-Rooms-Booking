using FluentValidation;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Rooms.Application.ReadModels;

namespace MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms
{
    public sealed class GetRoomsHandler
    {
        private readonly IRoomQueries _rooms;
        private readonly IValidator<GetRoomsQuery> _validator;
        private readonly ILogger<GetRoomsHandler> _logger;

        public GetRoomsHandler(
            IRoomQueries rooms,
            IValidator<GetRoomsQuery> validator,
            ILogger<GetRoomsHandler> logger)
        {
            _rooms = rooms;
            _validator = validator;
            _logger = logger;
        }


        public async Task<Result<List<RoomReadModel>>> Handle(GetRoomsQuery query, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(query, ct);

            if (!validation.IsValid)
            {
                _logger.LogWarning(
                    "GetRooms validation failed. Error: {Error}",
                    validation.Errors.First().ErrorMessage);

                return Result<List<RoomReadModel>>.BadRequest(
                    "VALIDATION_ERROR", validation.Errors.First().ErrorMessage);
            }

            _logger.LogInformation(
                "Fetching rooms list. Filters: Name={Name}, Location={Location}, " +
                "MinCapacity={MinCapacity}, IsActive={IsActive}",
                query.Name,
                query.Location,
                query.MinCapacity,
                query.IsActive);
            
            var result = await _rooms.GetByFiltersAsync(query, ct);

            _logger.LogInformation("Rooms fetched successfully. Count: {Count}", result.Count);

            return Result<List<RoomReadModel>>.Ok(result);
        }

    }
}
