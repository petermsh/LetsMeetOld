using System.Net;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.API.Exceptions;

public class UserNotFoundException : ProjectException
{
    public UserNotFoundException(string login) : base($"Użytkownik {login} nie istnieje.", HttpStatusCode.NotFound) { }
}

public class UsersNotFoundException : ProjectException
{
    public UsersNotFoundException() : base("Nie znaleziono użytkowników", HttpStatusCode.NotFound) { }
}

public class UserWrongPasswordException : ProjectException
{
    public UserWrongPasswordException() : base($"Wprowadzone hasło jest nieprawidłowe.", HttpStatusCode.NotFound) { }
}

public class UserWrongRepeatedPasswordException : ProjectException
{
    public UserWrongRepeatedPasswordException() : base("Wprowadzone hasla nie są zgodne.") { }
}

public class UserEmailAlreadyExistException : ProjectException
{
    public UserEmailAlreadyExistException(string email) : base($"Konto o adresie mailowym {email} już istnieje.") { }
}

public class UserNameAlreadyExistException : ProjectException
{
    public UserNameAlreadyExistException(string nick) : base($"Nick {nick} jest już zajęty.") { }
}

public class RegistrationFailedException : ProjectException
{
    public RegistrationFailedException(string errors) : base(errors)
    {
    }
}