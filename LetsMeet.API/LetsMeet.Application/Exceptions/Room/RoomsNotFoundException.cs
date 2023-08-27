namespace LetsMeet.Application.Exceptions.Room;

public class RoomsNotFoundException : ProjectException
{
    public RoomsNotFoundException() : base("Nie znaleziono żadnego pokoju")
    {
    }
}