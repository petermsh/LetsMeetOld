using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Account;

namespace LetsMeet.Application.Commands.Account.ChangePassword;

public record ChangePasswordCommand(Guid Id, string NewPassword, string ConfirmNewPassword, string Token) : ICommand;