using System.Net;

namespace LetsMeet.Application.Exceptions.Message;

public class MessageNotFoundException : ProjectException
{
    public MessageNotFoundException(HttpStatusCode errorCode = HttpStatusCode.NotFound) : base("Wiadomość nie została znaleziona ")
    {
    }
}