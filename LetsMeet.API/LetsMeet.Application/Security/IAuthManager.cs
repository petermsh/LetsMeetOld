using LetsMeet.Core.Domain.Entities;

namespace LetsMeet.Application.Security;

public interface IAuthManager
{
    string CreateToken(User user);
}