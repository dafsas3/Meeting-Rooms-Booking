using MeetingRoomsBooking.Features.Abstractions.Common.Errors;
using MeetingRoomsBooking.Features.Abstractions.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
                var error = new ApiError(ex.Code, ex.Message, ex.StatusCode, ex.Meta);

                await WriteProblemAsync(ctx, error);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occured");

                var error = new ApiError(
                    Code: "INTERNAL_SERVER_ERROR",
                    Message: "An unexpected error occurred.",
                    StatusCode: 500);

                await WriteProblemAsync(ctx, error);
            }
        }


        private static async Task WriteProblemAsync(HttpContext ctx, ApiError error)
        {
            if (ctx.Response.HasStarted)
                throw new InvalidOperationException("The response has already started.");

            ctx.Response.Clear();
            ctx.Response.StatusCode = error.StatusCode;
            ctx.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = error.StatusCode,
                Title = error.Message,
                Detail = error.Message,
                Type = error.Code
            };

            problem.Extensions["code"] = error.Code;
            problem.Extensions["traceId"] = ctx.TraceIdentifier;

            if (error.Details is not null)
                problem.Extensions["details"] = error.Details;

            await ctx.Response.WriteAsJsonAsync(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

    }
}
