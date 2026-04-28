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
                "INVALID_BOOKING_STATUS_TRANSFER_REASON" => StatusCodes.Status400BadRequest,
                "INVALID_MEETING_PURPOSE" => StatusCodes.Status400BadRequest,
                "INVALID_PARTICIPANT_EMAIL" => StatusCodes.Status400BadRequest,
                "INVALID_TIMESLOT_START" => StatusCodes.Status400BadRequest,
                "INVALID_TIMESLOT_END" => StatusCodes.Status400BadRequest,
                "INVALID_TIMESLOT_RANGE" => StatusCodes.Status400BadRequest,
                "INVALID_TIMESLOT_DURATION" => StatusCodes.Status400BadRequest,
                "INVALID_BOOKING_HISTORY_ID" => StatusCodes.Status400BadRequest,
                "INVALID_BOOKING_REQUEST_ID" => StatusCodes.Status400BadRequest,
                "INVALID_EMPLOYEE_ID" => StatusCodes.Status400BadRequest,
                "INVALID_BOOKING_STATUS_TRANSITION" => StatusCodes.Status409Conflict,
                "INVALID_MISMATCH_EMPLOYEE_ID" => StatusCodes.Status403Forbidden,

                _ => StatusCodes.Status400BadRequest
            };
    }
}