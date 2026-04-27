using MeetingRoomsBooking.Api.Extensions;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.CreateBookingRequest;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Submit;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Features.Bookings.Api.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CreateBookingRequestHandler _createBookingHandler;
        private readonly SubmitBookingHandler _submitHandler;

        public BookingsController(
            CreateBookingRequestHandler bookingRequest,
            SubmitBookingHandler submit)
        {
            _createBookingHandler = bookingRequest;
            _submitHandler = submit;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBookingRequestCommand cmd,
            [FromHeader(Name = "Idempotency-Key")] Guid idempotencyKey,
            CancellationToken ct)
        {
            return this.ToActionResult(await _createBookingHandler.Handle(cmd, idempotencyKey, ct));
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(int id, CancellationToken ct)
        {
            return this.ToActionResult(await _submitHandler.Handle(new SubmitBookingCommand(id), ct));
        }

    }
}
