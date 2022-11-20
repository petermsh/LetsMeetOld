using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.API.Controllers;

[ApiController]
[Authorize]
public class RoomController : Controller
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPatch("update")]
    public IActionResult ChangeRoomStatus([FromBody] bool isLocked, [FromQuery] string roomId)
    {
        _roomService.ChangeRoomStatus(isLocked, roomId);
        return Ok();
    }
}