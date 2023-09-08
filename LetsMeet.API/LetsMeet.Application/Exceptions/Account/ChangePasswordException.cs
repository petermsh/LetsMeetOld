using System.Net;

namespace LetsMeet.Application.Exceptions.Account;

public class ChangePasswordException : ProjectException
{
    public ChangePasswordException() : base("Zmiana hasła nie powiodła się") 
    {
    }
}