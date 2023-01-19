using System.ComponentModel.DataAnnotations;
using MimeKit;

namespace LetsMeet.API.DTO;

public class ResponseViewModel
{
    public bool Status { get; init; }
    public string Message { get; init; }
    public string StatusCode { get; init; }
    public string Data { get; init; }
}

public class EmailTemplate
{
    public string Link { get; set; }
    public string UserId { get; set; }
    
}

public class EmailAddress
{
    public string Address { get; set; }
    public string DisplayName { get; set; }
}

public class EmailMessage
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    
    public EmailMessage(IEnumerable<EmailAddress> to, string subject, string content)     
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
        Subject = subject;
        Content = content;        
    }
    
    public class EmailConfiguration
    {
        public string From { get; init; }
        public string SmtpServer { get; init; }
        public int Port { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}

