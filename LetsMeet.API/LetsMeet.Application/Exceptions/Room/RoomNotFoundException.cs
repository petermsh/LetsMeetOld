namespace LetsMeet.Application.Exceptions.Room;

public class RoomNotFoundException : ProjectException
{
    public RoomNotFoundException() : base("Pokoj nie zostal znaleziony")
    {
    }
}