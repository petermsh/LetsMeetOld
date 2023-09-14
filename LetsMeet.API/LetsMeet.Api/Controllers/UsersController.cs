using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.User.ChangeInformations;
using LetsMeet.Application.Commands.User.ChangeProfilePhoto;
using LetsMeet.Application.Queries.User.GetCurrentUser;
using LetsMeet.Application.Queries.User.GetUserByUserName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LetsMeet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IQueryHandler<GetUserByUserNameQuery, UserDetailsDto> _getUserByUserNameHandler;
    private readonly IQueryHandler<GetCurrentUserQuery, UserDetailsDto> _getCurrentUserHandler;
    private readonly ICommandHandler<ChangeInformationsCommand> _changeUserInformationHandler;
    private readonly ICommandHandler<ChangeUserPhotoCommand> _changeUserPhotoHandler;

    public UsersController(IQueryHandler<GetUserByUserNameQuery, UserDetailsDto> getUserByUserNameHandler, IQueryHandler<GetCurrentUserQuery, UserDetailsDto> getCurrentUserHandler, ICommandHandler<ChangeInformationsCommand> changeUserInformationHandler, ICommandHandler<ChangeUserPhotoCommand> changeUserPhotoHandler)
    {
        _getUserByUserNameHandler = getUserByUserNameHandler;
        _getCurrentUserHandler = getCurrentUserHandler;
        _changeUserInformationHandler = changeUserInformationHandler;
        _changeUserPhotoHandler = changeUserPhotoHandler;
    }

    [HttpGet("name")]
    [Authorize]
    [SwaggerOperation(
        Summary="Get User by name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailsDto>> GetUserByUserName([FromQuery]GetUserByUserNameQuery query)
    {
        return await _getUserByUserNameHandler.HandleAsync(query);
    }
    
    [HttpGet("me")]
    [Authorize]
    [SwaggerOperation(
        Summary="Get current User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailsDto>> GetCurrentUser()
    {
        var query = new GetCurrentUserQuery(User.Identity.Name);
        return await _getCurrentUserHandler.HandleAsync(query);
    }
    
    [HttpPatch("change-informations")]
    [Authorize]
    [SwaggerOperation(
        Summary="Change User informations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ChangeUserInformations([FromBody] ChangeInformationsCommand command)
    {
        await _changeUserInformationHandler.HandleAsync(command);
        return NoContent();
    }
    
    [HttpPatch("change-user-photo")]
    [Authorize]
    [SwaggerOperation(
        Summary="Change User Photo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ChangeUserPhoto([FromForm] ChangeUserPhotoCommand command)
    {
        await _changeUserPhotoHandler.HandleAsync(command);
        return NoContent();
    }
}