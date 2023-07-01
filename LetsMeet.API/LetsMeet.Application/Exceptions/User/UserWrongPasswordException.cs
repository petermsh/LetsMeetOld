using System.Net;

namespace LetsMeet.Application.Exceptions.User;

public class UserWrongPasswordException : ProjectException
{
    public UserWrongPasswordException() : base($"Wprowadzone hasło jest nieprawidłowe.", HttpStatusCode.BadRequest) { }
}