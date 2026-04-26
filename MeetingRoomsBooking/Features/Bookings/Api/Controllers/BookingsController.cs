using FluentValidation;
using MeetingRoomsBooking.Api.Extensions;
using MeetingRoomsBooking.Features.Abstractions.Common.Result;
using MeetingRoomsBooking.Features.Bookings.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Features.Bookings.Api.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CreateBookingRequestHandler _bookingRequest;

        public BookingsController(
            CreateBookingRequestHandler bookingRequest)
        {
            _bookingRequest = bookingRequest;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBookingRequestCommand cmd,
            [FromHeader(Name = "Idempotency-Key")] Guid idempotencyKey,
            CancellationToken ct)
        {          
            return this.ToActionResult(await _bookingRequest.Handle(cmd, idempotencyKey, ct));
        }
    }
}
