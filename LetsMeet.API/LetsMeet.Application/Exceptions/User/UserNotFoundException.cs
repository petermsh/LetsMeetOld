using System.Net;

namespace LetsMeet.Application.Exceptions.User;

public class UserNotFoundException : ProjectException
{
    public UserNotFoundException(string login) : base($"Użytkownik {login} nie istnieje.", HttpStatusCode.NotFound) { }
    
    public UserNotFoundException() : base($"Użytkownik nie istnieje.", HttpStatusCode.NotFound) { }

}
