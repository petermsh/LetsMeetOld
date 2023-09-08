using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.User.ChangeInformations;
using LetsMeet.Application.Exceptions.Account;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Application.Commands.Account.ChangePassword;

public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly UserManager<Core.Domain.Entities.User> _userManager;

    public ChangePasswordHandler(IUserRepository userRepository, IUserInfoProvider userInfoProvider, UserManager<Core.Domain.Entities.User> userManager)
    {
        _userRepository = userRepository;
        _userInfoProvider = userInfoProvider;
        _userManager = userManager;
    }

    public async Task HandleAsync(ChangePasswordCommand command)
    {
        var user = await _userRepository.GetByUsernameAsync(_userInfoProvider.UserName);

        if (user is null)
            throw new UserNotFoundException("");

        if (command.NewPassword != command.ConfirmNewPassword)
            throw new WrongRepeatedPasswordException();
        
        var result = await _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);

        if (!result.Succeeded)
            throw new ChangePasswordException();
    }
}