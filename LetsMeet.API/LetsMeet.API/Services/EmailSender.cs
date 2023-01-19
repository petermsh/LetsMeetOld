using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Infrastructure;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace LetsMeet.API.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailMessage.EmailConfiguration _emailConfig;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public EmailSender(EmailMessage.EmailConfiguration emailConfig, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _emailConfig = emailConfig;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        var emailMessage = CreateEmailMessage(message);

        await SendAsync(emailMessage);
    }

    // public async Task ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    // {
    //     var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
    //     var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //     var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
    //     var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
    //     await _emailSender.SendEmailAsync(message);
    // }
    
    // public async Task<ResponseViewModel> SendResetPwdLink(string email)
    // {
    //    try
    //    {
    //       var user = await _userManager.FindByEmailAsync(email);
    //       var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //       var link = "https://localhost:44379/api/account/confirmresetpassword?";
    //       var buillink = link  + "&Id=" + user.Id + "&token=" + token;
    //       var emailtemplate = new EmailTemplate();
    //        emailtemplate.Link = buillink;
    //        emailtemplate.UserId = user.Id;
    //        emailtemplate.EmailType = EmailType.ResetPassword;
    //        var emailsent = _emailService.SendSmtpMail(emailtemplate);
    //        if(emailsent != true)
    //           throw new HttpStatusException(System.Net.HttpStatusCode.InternalServerError, "Email not sent.");
    //
    //        return new ResponseViewModel
    //        {
    //            Status = true,
    //            Message = "Link Sent Succesfully",
    //            StatusCode = System.Net.HttpStatusCode.OK.ToString(),
    //            Data = buillink
    //         };
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;
    //     }
    // }

    private MimeMessage CreateEmailMessage(EmailMessage message)
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
        using (var client = new SmtpClient())
        {
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
}