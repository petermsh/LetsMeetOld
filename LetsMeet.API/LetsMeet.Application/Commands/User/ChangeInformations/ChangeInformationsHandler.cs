using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Core.Domain.Enums;
using LetsMeet.Core.Domain.Repositories;

namespace LetsMeet.Application.Commands.User.ChangeInformations;

public class ChangeInformationsHandler : ICommandHandler<ChangeInformationsCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserInfoProvider _userInfoProvider;

    public ChangeInformationsHandler(IUserRepository userRepository, IUserInfoProvider userInfoProvider)
    {
        _userRepository = userRepository;
        _userInfoProvider = userInfoProvider;
    }

    public async Task HandleAsync(ChangeInformationsCommand command)
    {
        var user = await _userRepository.GetByUsernameAsync(_userInfoProvider.UserName);

        if (user is null)
            throw new UserNotFoundException("");

        user.UserName = command.UserName;
        user.Bio = command.Bio;
        user.Gender = (Gender)command.Gender;
        user.City = command.City;
        user.University = command.University;
        user.Major = command.Major;

        await _userRepository.UpdateAsync(user);
    }
}