using LetsMeet.API.Database.Entities;

namespace LetsMeet.API.Interfaces;

public interface IAuthManager
{
    string CreateToken(User user);
}