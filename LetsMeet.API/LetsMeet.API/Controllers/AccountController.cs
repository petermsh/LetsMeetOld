using AutoMapper;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.API.Controllers;

[ApiController]
public class AccountController : Controller
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAuthManager _authManager;
    public AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IAuthManager authManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _authManager = authManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserReturnDto>> Register(UserRegDto registerDto)
    {
        if (await UserExists(registerDto.Nick)) return BadRequest("Username is taken");

        var user = _mapper.Map<User>(registerDto);

        user.UserName = registerDto.Nick.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return new UserReturnDto
        {
            Nick = user.UserName,
            Token = _authManager.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserReturnDto>> Login(UserLoginDto loginDto)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Login.ToLower());

        if (user is null) return Unauthorized("Invalid username");
        
        var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized();

        return new UserReturnDto
        {
            Nick = user.UserName,
            Token = _authManager.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}