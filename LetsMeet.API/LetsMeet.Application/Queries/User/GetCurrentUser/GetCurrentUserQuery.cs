using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Queries.User.GetUserByUserName;

namespace LetsMeet.Application.Queries.User.GetCurrentUser;

public record GetCurrentUserQuery(string Username) : IQuery<UserDetailsDto>
{
}