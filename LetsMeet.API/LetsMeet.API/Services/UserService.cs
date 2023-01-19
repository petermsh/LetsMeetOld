using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Enums;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Hubs;
using LetsMeet.API.Infrastructure;
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
    private readonly IEmailSender _emailSender;

    public UserService(DataContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager, IAuthManager authManager, IUserInfoProvider userInfoProvider, IEmailSender emailSender)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _authManager = authManager;
        _userInfoProvider = userInfoProvider;
        _emailSender = emailSender;
    }

    public UserInfoDto GetByName(string userName)
    {
        var user = _dataContext.Users.FirstOrDefault(x => x.UserName == userName);
        if (user is null)
        {
            throw new UserNotFoundException(userName);
        }

        var userInfo = new UserInfoDto
        {
            UserName = user.UserName,
            Bio = user.Bio,
            City = user.City,
            Major = user.Major,
            University = user.University
        };

        return userInfo;
    }

    public async Task<UserReturnDto> Register(UserRegDto userRegDto)
    {
        if (await UserNameExists(userRegDto.UserName))
        {
            throw new UserNameAlreadyExistException(userRegDto.UserName);
        }
        
        if (await UserEmailExists(userRegDto.Email))
        {
            throw new UserEmailAlreadyExistException(userRegDto.Email);
        }

        var user = new User
        {
            UserName = userRegDto.UserName,
            Email = userRegDto.Email,
            Bio = userRegDto.Bio,
            City = userRegDto.City,
            Gender = (Gender)userRegDto.Gender,
            University = userRegDto.University,
            Major = userRegDto.Major,
        };
        var register = await _userManager.CreateAsync(user, userRegDto.Password);
        if (!register.Succeeded)
        {
            var errors = string.Join(" ", register.Errors.Select(x => x.Description).ToList());
            throw new RegistrationFailedException(errors);
        }

        return new UserReturnDto
        {
            UserName = user.UserName,
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
            throw new UserWrongPasswordException();
        }

        return new UserReturnDto
        {
            UserName = user.UserName,
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

    public async Task UpdateInfo(UserEditDto userEditDto)
    {
        var user = _userInfoProvider.CurrentUser;
        if (user is null)
        {
            throw new UserNotFoundException("");
        }

        user.UserName = userEditDto.UserName;
        user.Bio = userEditDto.Bio;
        user.City = userEditDto.City;
        user.University = userEditDto.University;
        user.Major = userEditDto.Major;
        user.Gender = (Gender)userEditDto.Gender;

        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<ResponseViewModel> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var link = "https://localhost:7168/change-password?";
        var buillink = "Kliknij poniższy link, aby zmienić hasło. " + '\n' + link  + "&Id=" + user.Id + "&token=" + token;

        var message = new EmailMessage(
            new []
                { new EmailAddress() { Address = forgotPasswordDto.Email, DisplayName = user.UserName } },
            "Change password", buillink);

        await _emailSender.SendEmailAsync(message);
        return new ResponseViewModel()
        {
            Status = true,
            Message = "Link Sent Succesfully",
            StatusCode = System.Net.HttpStatusCode.OK.ToString(),
            Data = buillink
        };
    }

    public async Task<string> ChangePassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByIdAsync(resetPasswordDto.Id);
        if (user is null)
            throw new UserNotFoundException("");

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
        if (!result.Succeeded)
            throw new Exception("Zmiana hasła zakońćzona niepowodzeniem");
        else
            return "Zmiana hasła zakonczona powodzeniem";
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