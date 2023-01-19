using System.ComponentModel;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Enums;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Infrastructure;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.API.Controllers;

[ApiController]
public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly IEmailSender _emailSender;
    public AccountController(IUserService userService, IEmailSender emailSender)
    {
        _userService = userService;
        _emailSender = emailSender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegDto registerDto)
    {
        var register = await _userService.Register(registerDto);
        return Ok(register);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto loginDto)
    {
        var login = await _userService.Login(loginDto);
        return Ok(login);
    }
    
    [HttpPost("forgot")]
    public async Task<IActionResult> ForgotPassword([FromQuery] ForgotPasswordDto forgotPasswordDto)
    {
        var result = await _userService.ForgotPassword(forgotPasswordDto);
        return Ok(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ResetPasswordDto resetPasswordDto)
    {
        var result = await _userService.ChangePassword(resetPasswordDto);
        return Ok(result);
    }
}