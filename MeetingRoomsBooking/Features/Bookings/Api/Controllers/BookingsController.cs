using MeetingRoomsBooking.Api.Extensions;
using MeetingRoomsBooking.Features.Bookings.Api.Requests;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Cancel;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Confirm;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.CreateBookingRequest;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Decline;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Submit;
using MeetingRoomsBooking.Features.Bookings.Application.Queries.GetBookings;
using MeetingRoomsBooking.Features.Bookings.Application.Queries.GetDetails;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Features.Bookings.Api.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CreateBookingRequestHandler _create;
        private readonly SubmitBookingHandler _submit;
        private readonly ConfirmBookingHandler _confirm;
        private readonly BookingDeclineHandler _decline;
        private readonly BookingCancelHandler _cancel;
        private readonly GetBookingDetailsHandler _getDetails;
        private readonly GetBookingsHandler _getBookings;


        public BookingsController(
            CreateBookingRequestHandler request,
            SubmitBookingHandler submit,
            ConfirmBookingHandler confirm,
            BookingDeclineHandler decline,
            BookingCancelHandler cancel,
            GetBookingDetailsHandler details,
            GetBookingsHandler bookings)
        {
            _create = request;
            _submit = submit;
            _confirm = confirm;
            _decline = decline;
            _cancel = cancel;
            _getDetails = details;
            _getBookings = bookings;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBookingRequestCommand cmd,
            [FromHeader(Name = "Idempotency-Key")] Guid idempotencyKey,
            CancellationToken ct)
        {
            return this.ToActionResult(await _create.Handle(cmd, idempotencyKey, ct));
        }


        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit([FromRoute] int id, CancellationToken ct)
        {
            return this.ToActionResult(await _submit.Handle(new SubmitBookingCommand(id), ct));
        }


        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm([FromRoute] int id, CancellationToken ct)
        {
            return this.ToActionResult(await _confirm.Handle(new ConfirmBookingCommand(id), ct));
        }


        [HttpPost("{id}/decline")]
        public async Task<IActionResult> Decline(
            [FromRoute] int id,
            [FromBody] BookingDeclineRequest req,
            CancellationToken ct)
        {
            var cmd = new BookingDeclineCommand(id, req.Reason);
            return this.ToActionResult(await _decline.Handle(cmd, ct));
        }


        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(
            [FromRoute] int id,
            [FromBody] BookingCancelRequest req,
            CancellationToken ct)
        {
            var cmd = new BookingCancelCommand(id, req.Reason);
            return this.ToActionResult(await _cancel.Handle(cmd, ct));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails([FromRoute] int id, CancellationToken ct)
        {
            return this.ToActionResult(await _getDetails.Handle(new GetBookingDetailsQuery(id), ct));
        }


        [HttpGet]
        public async Task<IActionResult> GetBookings(
            [FromQuery] GetBookingsQuery query,
            CancellationToken ct)
        {
            return this.ToActionResult(await _getBookings.Handle(query, ct));
        }
    }
}
