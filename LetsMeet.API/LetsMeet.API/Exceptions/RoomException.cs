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

public class RoomsNotFoundException : ProjectException
{
    public RoomsNotFoundException() : base("Nie ma żadnego pokoju na liście")
    {
    }
}

public class RoomExistsException : ProjectException
{
    public RoomExistsException() : base("Connection exists!")
    {
    }
}