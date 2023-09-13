using System.Net;
using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.Room.CreateRoom;
using LetsMeet.Application.Commands.Room.DeleteRoom;
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
    private readonly ICommandHandler<DeleteRoomCommand> _deleteRoomHandler;

    public RoomsController(ICommandHandler<CreateRoomCommand, CreatedRoomDto> createRoomHandler, ICommandHandler<DeleteRoomCommand> deleteRoomHandler)
    {
        _createRoomHandler = createRoomHandler;
        _deleteRoomHandler = deleteRoomHandler;
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
    
    [HttpDelete("delete")]
    [SwaggerOperation(Summary = "Delete Room")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRoom([FromQuery] DeleteRoomCommand command)
    {
        await _deleteRoomHandler.HandleAsync(command);
        return NoContent();
    }
}