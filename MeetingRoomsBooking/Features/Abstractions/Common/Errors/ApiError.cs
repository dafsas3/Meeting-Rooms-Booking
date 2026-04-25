namespace MeetingRoomsBooking.Features.Abstractions.Common.Errors
{
    public sealed record ApiError(
        string Code,
        string Message,
        object? Details = null);
}
