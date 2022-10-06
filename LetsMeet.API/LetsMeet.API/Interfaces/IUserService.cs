using LetsMeet.API.DTO;

namespace LetsMeet.API.Interfaces;

public interface IUserService
{
    void CreateUser(UserRegDto userRegDto);
    string Login(UserLoginDto dto);
    UserInfoDto GetInfo();
}