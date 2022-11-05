using AutoMapper;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Helper;
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
public class UserController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        var users = await _userService.GetUsers();
        var userlist = _mapper.Map<IEnumerable<UserInfoDto>>(users);
        return Ok(userlist);
    }
}