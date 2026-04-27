using MeetingRoomsBooking.Api.Extensions;
using MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom;
using MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomsBooking.Features.Rooms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly CreateRoomHandler _createRoom;
        private readonly GetRoomsHandler _getRooms;

        public RoomsController(
            CreateRoomHandler create,
            GetRoomsHandler get)
        {
            _createRoom = create;
            _getRooms = get;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRoomCommand cmd, CancellationToken ct)
        {
            return this.ToActionResult(await _createRoom.Handle(cmd, ct));
        }


        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetRoomsQuery query,  CancellationToken ct)
        {
            return this.ToActionResult(await _getRooms.Handle(query, ct));
        }
    }
}
