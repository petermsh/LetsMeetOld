using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Email;
using MailKit.Net.Smtp;
using MimeKit;

namespace LetsMeet.Infrastructure.Services.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfig;

    public EmailSender(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public async Task SendEmailAsync(EmailMessageDto message)
    {
        var emailMessage = CreateEmailMessage(message);

        await SendAsync(emailMessage);
    }
    
    private MimeMessage CreateEmailMessage(EmailMessageDto message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("LetsMeet", _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

        return emailMessage;
    }
    
    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

            await client.SendAsync(mailMessage);
        }
        catch
        {
            //log an error message or throw an exception or both.
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}