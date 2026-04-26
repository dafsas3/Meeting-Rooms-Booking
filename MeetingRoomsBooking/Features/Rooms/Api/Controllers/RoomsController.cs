using MeetingRoomsBooking.Api.Extensions;
using MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Features.Rooms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly CreateRoomHandler _createRoom;

        public RoomsController(
            CreateRoomHandler create)
        {
            _createRoom = create;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRoomCommand cmd, CancellationToken ct)
        {
            return this.ToActionResult(await _createRoom.Handle(cmd, ct));
        }
    }
}
