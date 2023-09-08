using LetsMeet.Application.DTO.Email;

namespace LetsMeet.Application.Abstractions;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessageDto message);
}