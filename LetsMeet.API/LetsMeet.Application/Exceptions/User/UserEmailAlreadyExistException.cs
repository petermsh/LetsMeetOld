namespace LetsMeet.Application.Exceptions.User;

public class UserEmailAlreadyExistException : ProjectException
{
    public UserEmailAlreadyExistException(string email) : base($"Konto o adresie mailowym {email} już istnieje.") { }
}