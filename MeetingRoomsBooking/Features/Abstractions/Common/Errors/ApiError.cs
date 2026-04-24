namespace MeetingRoomsBooking.Features.Abstractions.Common.Errors
{
    public sealed record ApiError(
        string Code,
        string Message,
        int StatusCode,
        object? Details = null);
}
