using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.User;
using LetsMeet.Application.Exceptions.Account;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Application.Security;
using LetsMeet.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Application.Commands.Account.SignUp;

internal sealed class SignUpHandler : ICommandHandler<SignUpCommand, UserLoggedDto>
{
    private readonly UserManager<Core.Domain.Entities.User> _userManager;
    private readonly IAuthManager _authManager;

    public SignUpHandler(UserManager<Core.Domain.Entities.User> userManager, IAuthManager authManager)
    {
        _userManager = userManager;
        _authManager = authManager;
    }

    public async Task<UserLoggedDto> HandleAsync(SignUpCommand command)
    {
        if (await UserNameExists(command.UserName))
            throw new UserNameAlreadyExistException(command.UserName);
        
        
        if (await UserEmailExists(command.Email))
            throw new UserEmailAlreadyExistException(command.Email);
        

        if (command.Password != command.RepeatedPassword)
            throw new WrongRepeatedPasswordException();

        var user = Core.Domain.Entities.User.Create(command.Email, command.UserName, command.Bio, command.City, command.University, command.Major, (Gender)command.Gender);

        var registeredUser = await _userManager.CreateAsync(user, command.Password);
        if (!registeredUser.Succeeded)
        {
            var errors = string.Join(" ", registeredUser.Errors.Select(x => x.Description).ToList());
            throw new RegistrationFailedException(errors);
        }

        return new UserLoggedDto()
        {
            UserName = user.UserName,
            Token = _authManager.CreateToken(user)
        };
    }
    
    private async Task<bool> UserNameExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username);
    }
    
    private async Task<bool> UserEmailExists(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }
}