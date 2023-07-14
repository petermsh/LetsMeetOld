using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.User;
using LetsMeet.Application.Commands.User.SignIn;
using LetsMeet.Application.Commands.User.SignUp;
using LetsMeet.Application.DTO.User;
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
    private readonly ICommandHandler<SignUpCommand, UserLoggedDto> _signUpHandler;
    private readonly ICommandHandler<SignInCommand, UserLoggedDto> _signInHandler;
    private readonly IQueryHandler<GetUserByUserNameQuery, UserDetailsDto> _getUserByUserNameHandler;
    private readonly IQueryHandler<GetCurrentUserQuery, UserDetailsDto> _getCurrentUserHandler;

    public UsersController(ICommandHandler<SignUpCommand, UserLoggedDto> signUpHandler, ICommandHandler<SignInCommand, UserLoggedDto> signInHandler, IQueryHandler<GetUserByUserNameQuery, UserDetailsDto> getUserByUserNameHandler, IQueryHandler<GetCurrentUserQuery, UserDetailsDto> getCurrentUserHandler)
    {
        _signUpHandler = signUpHandler;
        _signInHandler = signInHandler;
        _getUserByUserNameHandler = getUserByUserNameHandler;
        _getCurrentUserHandler = getCurrentUserHandler;
    }

    [HttpPost("signUp")]
    [SwaggerOperation(
        Summary="Sign Up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserLoggedDto>> SignUp(SignUpCommand command)
    {
        return await _signUpHandler.HandleAsync(command);
    }
    
    
    [HttpPost("signIn")]
    [SwaggerOperation(
        Summary="Sign In")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserLoggedDto>> SignIn(SignInCommand command)
    {
        return await _signInHandler.HandleAsync(command);
    }
    
    [HttpGet("name")]
    [Authorize]
    [SwaggerOperation(
        Summary="Get user by name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailsDto>> GetUserByUserName([FromQuery]GetUserByUserNameQuery query)
    {
        return await _getUserByUserNameHandler.HandleAsync(query);
    }
    
    [HttpGet("me")]
    [Authorize]
    [SwaggerOperation(
        Summary="Get current user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailsDto>> GetCurrentUser()
    {
        var query = new GetCurrentUserQuery(User.Identity.Name);
        return await _getCurrentUserHandler.HandleAsync(query);
    }
}