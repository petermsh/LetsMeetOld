using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace LetsMeet.Application.Commands.User.ChangeProfilePhoto;

public class ChangeUserPhotoHandler : ICommandHandler<ChangeUserPhotoCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ChangeUserPhotoHandler(IUserRepository userRepository, IUserInfoProvider userInfoProvider, IWebHostEnvironment webHostEnvironment)
    {
        _userRepository = userRepository;
        _userInfoProvider = userInfoProvider;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task HandleAsync(ChangeUserPhotoCommand command)
    {
        var user = await _userRepository.GetByUsernameAsync(_userInfoProvider.UserName);

        if (user is null)
            throw new UserNotFoundException("");

        using var memoryStream = new MemoryStream();
        await command.Photo.CopyToAsync(memoryStream);
        var photoBytes = memoryStream.ToArray();
        
        var base64String = Convert.ToBase64String(photoBytes);
        //user.Photo = memoryStream.ToArray();
        await _userRepository.UpdateAsync(user);
    }
}