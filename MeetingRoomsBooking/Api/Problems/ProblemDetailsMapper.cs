using MeetingRoomsBooking.Features.Abstractions.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Api.Problems
{
    public sealed class ProblemDetailsMapper
    {
        public static ProblemDetails ToProblemDetails(
            ApiError error,
            int statusCode,
            string traceId)
        {
            return new ProblemDetails
            {
                Status = statusCode,
                Title = error.Message,
                Type = error.Code,
                Detail = error.Message,
                Extensions =
                {
                    ["code"] = error.Code,
                    ["traceId"] = traceId,
                    ["details"] = error.Details
                }
            };
        }
    }
}
