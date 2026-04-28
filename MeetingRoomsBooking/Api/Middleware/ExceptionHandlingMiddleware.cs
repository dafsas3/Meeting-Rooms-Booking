using MeetingRoomsBooking.Api.Problems;
using MeetingRoomsBooking.Features.Abstractions.Common.Errors;
using MeetingRoomsBooking.Features.Abstractions.Domain;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }

            catch (DomainException ex)
            {
                var error = new ApiError(ex.Code, ex.Message, ex.Meta);

                await WriteProblemAsync(ctx, error, DomainErrorStatusMapper.Map(ex.Code));
            }

            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access.");

                var error = new ApiError(
                    Code: "INVALID_AUTH",
                    Message: ex.Message);

                await WriteProblemAsync(ctx, error, StatusCodes.Status401Unauthorized);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                var error = new ApiError(
                    Code: "INTERNAL_SERVER_ERROR",
                    Message: "An unexpected error occurred.");

                await WriteProblemAsync(ctx, error, StatusCodes.Status500InternalServerError);
            }
        }


        private static async Task WriteProblemAsync(HttpContext ctx, ApiError error, int statusCode)
        {
            if (ctx.Response.HasStarted)
                throw new InvalidOperationException("The response has already started.");

            ctx.Response.Clear();
            ctx.Response.StatusCode = statusCode;
            ctx.Response.ContentType = "application/problem+json";  

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = error.Message,
                Detail = error.Message,
                Type = error.Code
            };

            problem.Extensions["code"] = error.Code;
            problem.Extensions["traceId"] = ctx.TraceIdentifier;

            if (error.Details is not null)
                problem.Extensions["details"] = error.Details;

            await ctx.Response.WriteAsJsonAsync(problem);
        }
    }
}
