using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Account;
using LetsMeet.Application.DTO.Email;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Application.Commands.Account.ForgotPassword;

public class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand, ForgotPasswordDto>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<Core.Domain.Entities.User> _userManager;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordHandler(IUserRepository userRepository, UserManager<Core.Domain.Entities.User> userManager, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<ForgotPasswordDto> HandleAsync(ForgotPasswordCommand command)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is null)
            throw new UserNotFoundException("");
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var link = "https://localhost:7120/change-password?";
        var buillink = "Kliknij poniższy link, aby zmienić hasło. " + '\n' + link  + "&Id=" + user.Id + "&token=" + token;

        var message = new EmailMessageDto(
            new []
                { new EmailMessageDto.EmailAddress() { Address = command.Email, DisplayName = user.UserName } },
            "Change password", buillink);

        await _emailSender.SendEmailAsync(message);
        return new ForgotPasswordDto()
        {
            Status = true,
            Message = "Link Sent Succesfully",
            StatusCode = System.Net.HttpStatusCode.OK.ToString(),
            Data = buillink
        };
    }
}