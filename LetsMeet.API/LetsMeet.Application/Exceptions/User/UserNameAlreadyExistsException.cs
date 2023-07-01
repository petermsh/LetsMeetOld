namespace LetsMeet.Application.Exceptions.User;

public class UserNameAlreadyExistException : ProjectException
{
    public UserNameAlreadyExistException(string nick) : base($"Nick {nick} jest już zajęty.") { }
}