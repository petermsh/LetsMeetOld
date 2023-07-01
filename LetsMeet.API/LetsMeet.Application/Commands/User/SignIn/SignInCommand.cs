using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.User;

namespace LetsMeet.Application.Commands.User.SignIn;

public record SignInCommand(string Login, string Password) : ICommand<UserLoggedDto>
{
}