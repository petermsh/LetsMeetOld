using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Queries.User.GetUserByUserName;

public record GetUserByUserNameQuery : IQuery<UserDetailsDto>
{
    public string UserName { get; set; }
}