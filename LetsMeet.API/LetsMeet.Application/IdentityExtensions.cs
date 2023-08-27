using System.Security.Claims;
using LetsMeet.Application.Exceptions.User;

namespace LetsMeet.Application;

public static class IdentityExtensions
{
    public static string GetCurrentUserId(this ClaimsPrincipal user)
    {
        if (user.Identity == null || !user.Identity.IsAuthenticated)
            throw new UserNotFoundException("");

        var userIdString = user.FindFirst(ClaimTypes.NameIdentifier).Value;

        return userIdString;
    }
}