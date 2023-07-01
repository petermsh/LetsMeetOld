namespace LetsMeet.Application.Exceptions.User;

public class RegistrationFailedException : ProjectException
{
    public RegistrationFailedException(string errors) : base(errors)
    {
    }
}