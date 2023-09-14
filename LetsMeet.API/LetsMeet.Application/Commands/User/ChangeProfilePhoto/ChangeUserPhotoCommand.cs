using LetsMeet.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace LetsMeet.Application.Commands.User.ChangeProfilePhoto;

public class ChangeUserPhotoCommand : ICommand
{
    public IFormFile Photo { get; set; }
}