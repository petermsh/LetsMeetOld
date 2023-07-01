using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.User;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Application.Security;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Application.Commands.User.SignIn;

internal sealed class SignInHandler : ICommandHandler<SignInCommand, UserLoggedDto>
{
    private readonly UserManager<Core.Domain.Entities.User> _userManager;
    private readonly SignInManager<Core.Domain.Entities.User> _signInManager;
    private readonly IAuthManager _authManager;
    private readonly IUserRepository _userRepository;

    public SignInHandler(UserManager<Core.Domain.Entities.User> userManager, SignInManager<Core.Domain.Entities.User> signInManager, IUserRepository userRepository, IAuthManager authManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _authManager = authManager;
    }

    public async Task<UserLoggedDto> HandleAsync(SignInCommand command)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Login);
        
        if (user is null)
        {
            throw new UserNotFoundException(command.Login);
        }

        var login = await _signInManager.CheckPasswordSignInAsync(user, command.Password, false);

        if (!login.Succeeded)
        {
            throw new UserWrongPasswordException();
        }

        return new UserLoggedDto()
        {
            UserName = user.UserName,
            Token = _authManager.CreateToken(user)
        };
    }
}