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
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
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
        _logger.LogInformation("GetInfo executed..");
        
        var info = _userService.GetInfo();
        return Ok(info);
    }
    
    [Authorize]
    [HttpGet("{nick}")]
    public IActionResult GetUser(string nick)
    {
        var user = _userService.GetUser(nick);
        return Ok(user);
    }

    [Authorize]
    [HttpPatch("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult UpdateInfo([FromBody] UserEditDto userEditDto)
    {
        _userService.UpdateInfo(userEditDto);
        return NoContent();
    }
    

    [Authorize]
    [HttpPost("status")]
    public IActionResult ChangeStatus(bool status)
    {
        _userService.ChangeStatus(status);
        return NoContent();
    }
}