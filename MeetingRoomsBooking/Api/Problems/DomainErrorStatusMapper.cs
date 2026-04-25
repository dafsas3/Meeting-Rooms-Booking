namespace MeetingRoomsBooking.Api.Problems
{
    public static class DomainErrorStatusMapper
    {
        public static int Map(string code)
            =>
            code switch
            {
                "INVALID_ROOM_CAPACITY" => StatusCodes.Status400BadRequest,
                "INVALID_ROOM_LOCATION" => StatusCodes.Status400BadRequest,
                "INVALID_ROOM_NAME" => StatusCodes.Status400BadRequest,
                "INVALID_ROOM_ID" => StatusCodes.Status400BadRequest,
                "ROOM_NAME_ALREADY_EXISTS" => StatusCodes.Status409Conflict,

                _ => StatusCodes.Status400BadRequest
            };
    }
}