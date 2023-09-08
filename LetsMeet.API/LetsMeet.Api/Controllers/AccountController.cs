using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.Account.ChangePassword;
using LetsMeet.Application.Commands.Account.ForgotPassword;
using LetsMeet.Application.Commands.Account.SignIn;
using LetsMeet.Application.Commands.Account.SignUp;
using LetsMeet.Application.DTO.Account;
using LetsMeet.Application.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LetsMeet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ICommandHandler<SignUpCommand, UserLoggedDto> _signUpHandler;
    private readonly ICommandHandler<SignInCommand, UserLoggedDto> _signInHandler;
    private readonly ICommandHandler<ForgotPasswordCommand, ForgotPasswordDto> _forgotPasswordHandler;
    private readonly ICommandHandler<ChangePasswordCommand> _changePasswordHandler;

    public AccountController(ICommandHandler<SignUpCommand, UserLoggedDto> signUpHandler, ICommandHandler<SignInCommand, UserLoggedDto> signInHandler, ICommandHandler<ForgotPasswordCommand, ForgotPasswordDto> forgotPasswordHandler, ICommandHandler<ChangePasswordCommand> changePasswordHandler)
    {
        _signUpHandler = signUpHandler;
        _signInHandler = signInHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _changePasswordHandler = changePasswordHandler;
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
    
    [HttpPost("forgot-password")]
    [SwaggerOperation(
        Summary="Forgot password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ForgotPasswordDto>> ForgotPassword(ForgotPasswordCommand command)
    {
        return await _forgotPasswordHandler.HandleAsync(command);
    }
    
    [HttpPost("change-password")]
    [SwaggerOperation(
        Summary="Change password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ForgotPasswordDto>> Change(ChangePasswordCommand command)
    {
        await _changePasswordHandler.HandleAsync(command);
        return NoContent();
    }
}