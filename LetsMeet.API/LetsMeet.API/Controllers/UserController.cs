using AutoMapper;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Hubs;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace LetsMeet.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        var user = _userService.GetInfo();
        return Ok(user);
    }

    [HttpPatch("update")]
    public IActionResult UpdateInfo([FromBody] UserEditDto userEditDto)
    {
        var user = _userService.UpdateInfo(userEditDto);
        return Ok();
    }
}