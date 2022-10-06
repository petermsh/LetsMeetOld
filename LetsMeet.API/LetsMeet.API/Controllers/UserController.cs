using LetsMeet.API.DTO;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult CreateUser([FromBody] UserRegDto userRegDto)
    {
        _userService.CreateUser(userRegDto);
        return NoContent();
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Login([FromBody] UserLoginDto dto)
    {
        var token = _userService.Login(dto);
        return Ok(new { Token = token });
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult GetInfo()
    {
        var info = _userService.GetInfo();
        return Ok(info);
    }
}