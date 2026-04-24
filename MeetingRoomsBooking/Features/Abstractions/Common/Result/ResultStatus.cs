namespace MeetingRoomsBooking.Features.Abstractions.Common.Result
{
    public enum ResultStatus
    {
        Ok,
        Created,
        Conflict,
        BadRequest,
        NotFound
    }
}
