using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Commands.User.ChangeInformations;

public record ChangeInformationsCommand(string UserName, string Bio, int Gender, string City, string University, string Major) : ICommand;