using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Commands.User.ChangeInformations;

public record ChangeInformationsCommand(string UserName, string Bio, string City, string University, string Major) : ICommand;