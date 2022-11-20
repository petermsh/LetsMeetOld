using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.API.Interfaces;

public interface IUserService
{
    Task<UserReturnDto> Register(UserRegDto userRegDto);
    Task<UserReturnDto> Login(UserLoginDto userRegDto);
    UserInfoDto GetInfo();
    Task UpdateInfo(UserEditDto userEditDto);
}