using LetsMeet.API.DTO;

namespace LetsMeet.API.Infrastructure;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage message);
}