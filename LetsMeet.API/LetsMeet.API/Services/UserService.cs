using AutoMapper;
using AutoMapper.QueryableExtensions;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Hubs;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.API.Services;

internal class UserService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAuthManager _authManager;
    private readonly IUserInfoProvider _userInfoProvider;

    public UserService(DataContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager, IAuthManager authManager, IUserInfoProvider userInfoProvider)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _authManager = authManager;
        _userInfoProvider = userInfoProvider;
    }

    public async Task<UserReturnDto> Register(UserRegDto userRegDto)
    {
        if (await UserNameExists(userRegDto.Nick))
        {
            throw new UserNameAlreadyExistException(userRegDto.Nick);
        }
        
        if (await UserEmailExists(userRegDto.Email))
        {
            throw new UserEmailAlreadyExistException(userRegDto.Email);
        }

        var user = new User
        {
            UserName = userRegDto.Nick,
            Email = userRegDto.Email,
            Bio = userRegDto.Bio,
            City = userRegDto.City,
            University = userRegDto.University,
            Major = userRegDto.Major,
        };
        var register = await _userManager.CreateAsync(user, userRegDto.Password);
        if (!register.Succeeded)
        {
            throw new Exception("Rejestracja nie powiodla sie");
        }

        return new UserReturnDto
        {
            Nick = user.UserName,
            Token = _authManager.CreateToken(user)
        };
    }

    public async Task<UserReturnDto> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.UserName == userLoginDto.Login);
        
        if (user is null)
        {
            throw new UserNotFoundException(userLoginDto.Login);
        }

        var login = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);

        if (!login.Succeeded)
        {
            throw new UnauthorizedAccessException();
        }

        return new UserReturnDto
        {
            Nick = user.UserName,
            Token = _authManager.CreateToken(user)
        };
    }

    public UserInfoDto GetInfo()
    {
        var user = _dataContext.Users.SingleOrDefault(x => x.UserName == _userInfoProvider.Name);
        if (user is null)
        {
            throw new UserNotFoundException("");
        }

        var info = new UserInfoDto
        {
            UserName = user.UserName,
            Bio = user.Bio,
            City = user.City,
            Major = user.Major,
            University = user.University
        };

        return info;
    }

    private async Task<bool> UserNameExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username);
    }
    
    private async Task<bool> UserEmailExists(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }
}