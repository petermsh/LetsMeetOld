namespace LetsMeet.API.Interfaces;

public interface IRoomService
{
    public void ChangeRoomStatus(bool isLocked, string roomId);
    public void DeleteRoom(string roomId);
}