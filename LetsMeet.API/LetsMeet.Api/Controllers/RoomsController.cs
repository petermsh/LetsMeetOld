using System.Net;
using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.Room.CreateRoom;
using LetsMeet.Application.DTO.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LetsMeet.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ICommandHandler<CreateRoomCommand, CreatedRoomDto> _createRoomHandler;

    public RoomsController(ICommandHandler<CreateRoomCommand, CreatedRoomDto> createRoomHandler)
    {
        _createRoomHandler = createRoomHandler;
    }

    [HttpPost("create")]
    [SwaggerOperation(Summary = "Create Room")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreatedRoomDto>> CreateRoom([FromQuery] CreateRoomCommand command)
    {
        command.ConnectionId = HttpContext.Connection.Id;
        return await _createRoomHandler.HandleAsync(command);
    }
}