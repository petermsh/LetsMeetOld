namespace LetsMeet.Application.Exceptions.Account;

public class WrongRepeatedPasswordException : ProjectException
{
    public WrongRepeatedPasswordException() : base("Wprowadzone hasla nie są zgodne.") { }
}