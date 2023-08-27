using System.Net;

namespace LetsMeet.Application.Exceptions.User;

public class UsersNotFoundException : ProjectException
{
    public UsersNotFoundException() : base("Nie znaleziono użytkowników", HttpStatusCode.NotFound) { }
}