using System.Net;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace LetsMeet.API.Exceptions;

public class RoomIsLockedException : ProjectException
{
    public RoomIsLockedException() : base("Chat został zablokowany")
    {
    }
}

public class RoomNotFoundException : ProjectException
{
    public RoomNotFoundException() : base("Pokoj nie zostal znaleziony")
    {
    }
}