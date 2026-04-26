using MeetingRoomsBooking.Features.Abstractions.Common.Errors;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Api.Extensions
{
    public static class ResultToActionResultExtensions
    {
        public static ApiError Internal() => new("INTERNAL_SERVER_ERROR", "Internal server Error");

        public static IActionResult ToActionResult<T>(
            this ControllerBase controller,
            Result<T> result)
            =>
            result.Status switch
            {
                ResultStatus.Ok => result.Data is null
                ? controller.NoContent()
                : controller.Ok(result.Data),

                ResultStatus.Created => result.Data is null
                ? controller.ToProblemDetails(Internal(), StatusCodes.Status500InternalServerError)
                : controller.StatusCode(StatusCodes.Status201Created, result.Data),

                ResultStatus.BadRequest => controller.ToProblemDetails(
                    result.Error ?? Internal(),
                    StatusCodes.Status400BadRequest),

                ResultStatus.Conflict => controller.ToProblemDetails(result.Error ?? Internal(),
                    StatusCodes.Status409Conflict),

                ResultStatus.NotFound => controller.ToProblemDetails(result.Error ?? Internal(),
                    StatusCodes.Status404NotFound),

                ResultStatus.Unauthorized => controller.ToProblemDetails(result.Error ?? Internal(),
                    StatusCodes.Status401Unauthorized),

                ResultStatus.Forbidden => controller.ToProblemDetails(result.Error ?? Internal(),
                    StatusCodes.Status403Forbidden),

                _ => controller.ToProblemDetails(Internal(), StatusCodes.Status500InternalServerError)
            };


        public static IActionResult ToProblemDetails(
            this ControllerBase controller,
            ApiError error,
            int statusCode)
        {
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = error.Message,
                Detail = error.Message,
                Type = error.Code
            };

            problem.Extensions["code"] = error.Code;
            problem.Extensions["traceId"] = controller.HttpContext.TraceIdentifier;

            if (error.Details is not null)
                problem.Extensions["details"] = error.Details;

            return new ObjectResult(problem)
            {
                StatusCode = statusCode,
                ContentTypes = { "application/problem+json" }
            };
        }
    }
}
