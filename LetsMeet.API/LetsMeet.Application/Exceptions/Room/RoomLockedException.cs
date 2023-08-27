namespace LetsMeet.Application.Exceptions.Room;

public class RoomLockedException : ProjectException
{
    public RoomLockedException() : base("Chat został zablokowany")
    {
    }
}