using LetsMeet.API.Database;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Interfaces;

namespace LetsMeet.API.Services;

public class RoomService : IRoomService
{
    private readonly DataContext _dataContext;

    public RoomService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public void ChangeRoomStatus(bool isLocked, string roomId)
    {
        var room = _dataContext.Rooms.FirstOrDefault(x => x.RoomId == roomId);

        room.isLocked = isLocked;

        _dataContext.Rooms.Update(room);
        _dataContext.SaveChanges();
    }

    public void DeleteRoom(string roomId)
    {
        var room = _dataContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
        if (room is null)
            throw new RoomNotFoundException();

        _dataContext.Rooms.Remove(room);
        _dataContext.SaveChanges();
    }
}