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

        var photo = await GetDefaultPhotoAsync();
        var user = Core.Domain.Entities.User.Create(command.Email, command.UserName, command.Bio, command.City, command.University, command.Major, (Gender)command.Gender, photo);

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

    private static async Task<string> GetDefaultPhotoAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync("https://cdn.discordapp.com/attachments/1040668916445360157/1151528918105477200/Group_6_1.png");

        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());
        }
        
        throw new Exception($"Błąd podczas pobierania obrazu: {response.StatusCode}");
    }
}