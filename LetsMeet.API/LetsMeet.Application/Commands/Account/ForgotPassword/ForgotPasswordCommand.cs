using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Account;

namespace LetsMeet.Application.Commands.Account.ForgotPassword;

public record ForgotPasswordCommand(string Email) : ICommand<ForgotPasswordDto>;