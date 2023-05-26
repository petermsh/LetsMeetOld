namespace LetsMeet.API.Exceptions;

public class MessageNotFoundException : ProjectException
{
    public MessageNotFoundException() : base("Wiadomość nie została znaleziona")
    {
    }
}