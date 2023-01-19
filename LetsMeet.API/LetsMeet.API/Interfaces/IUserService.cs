using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.API.Interfaces;

public interface IUserService
{
    UserInfoDto GetByName(string userName);
    Task<UserReturnDto> Register(UserRegDto userRegDto);
    Task<UserReturnDto> Login(UserLoginDto userRegDto);
    UserInfoDto GetInfo();
    Task UpdateInfo(UserEditDto userEditDto);
    Task<ResponseViewModel> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    Task<string> ChangePassword(ResetPasswordDto resetPasswordDto);
}