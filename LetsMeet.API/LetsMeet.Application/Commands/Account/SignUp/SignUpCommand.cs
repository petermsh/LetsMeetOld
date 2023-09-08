using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.User;

namespace LetsMeet.Application.Commands.Account.SignUp;

public record SignUpCommand(string Email, string UserName, string Password, string RepeatedPassword, string Bio, int Gender,
    string City, string University, string Major) : ICommand<UserLoggedDto>
{
}