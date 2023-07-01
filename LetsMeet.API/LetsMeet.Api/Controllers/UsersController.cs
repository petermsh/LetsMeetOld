using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.User;
using LetsMeet.Application.Commands.User.SignIn;
using LetsMeet.Application.Commands.User.SignUp;
using LetsMeet.Application.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ICommandHandler<SignUpCommand, UserLoggedDto> _signUpHandler;
    private readonly ICommandHandler<SignInCommand, UserLoggedDto> _signInHandler;

    public UsersController(ICommandHandler<SignUpCommand, UserLoggedDto> signUpHandler, ICommandHandler<SignInCommand, UserLoggedDto> signInHandler)
    {
        _signUpHandler = signUpHandler;
        _signInHandler = signInHandler;
    }

    [HttpPost("signUp")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserLoggedDto>> SignUp(SignUpCommand command)
    {
        return await _signUpHandler.HandleAsync(command);
    }
    
    
    [HttpPost("signIn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserLoggedDto>> SignIn(SignInCommand command)
    {
        return await _signInHandler.HandleAsync(command);
    }
}