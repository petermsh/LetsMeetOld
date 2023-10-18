using System.Net;
using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.Room.ChangeStatus;
using LetsMeet.Application.Commands.Room.CreateRoom;
using LetsMeet.Application.Commands.Room.DeleteRoom;
using LetsMeet.Application.DTO.Room;
using LetsMeet.Application.Queries.Room.GetRooms;
using LetsMeet.Application.Queries.Room.GetRoomsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;

namespace LetsMeet.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ICommandHandler<CreateRoomCommand, CreatedRoomDto> _createRoomHandler;
    private readonly ICommandHandler<DeleteRoomCommand> _deleteRoomHandler;
    private readonly IQueryHandler<GetRoomsListQuery, List<RoomsForListDto>> _getRoomsListHandler;
    private readonly ICommandHandler<ChangeRoomStatusCommand> _changeRoomStatusCommand;

    public RoomsController(ICommandHandler<CreateRoomCommand, CreatedRoomDto> createRoomHandler, ICommandHandler<DeleteRoomCommand> deleteRoomHandler, IQueryHandler<GetRoomsListQuery, List<RoomsForListDto>> getRoomsListHandler, ICommandHandler<ChangeRoomStatusCommand> changeRoomStatusCommand)
    {
        _createRoomHandler = createRoomHandler;
        _deleteRoomHandler = deleteRoomHandler;
        _getRoomsListHandler = getRoomsListHandler;
        _changeRoomStatusCommand = changeRoomStatusCommand;
    }

    [HttpPost("create")]
    [SwaggerOperation(Summary = "Create Room")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreatedRoomDto>> CreateRoom([FromQuery] CreateRoomCommand command)
    { 
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
    
    [HttpGet()]
    [SwaggerOperation(Summary = "Get Rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RoomsForListDto>>> GetRooms([FromQuery] GetRoomsListQuery command)
    {
        return await _getRoomsListHandler.HandleAsync(command);
    }
    
    [HttpPatch("change-status")]
    [SwaggerOperation(Summary = "Change Room status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ChangeRoomStatus([FromQuery] ChangeRoomStatusCommand command)
    {
        await _changeRoomStatusCommand.HandleAsync(command);
        return NoContent();
    }
}