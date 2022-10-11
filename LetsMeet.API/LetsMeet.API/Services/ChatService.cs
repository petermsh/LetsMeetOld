using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.Interfaces;

namespace LetsMeet.API.Services;

public class ChatService : IChatService
{
    private readonly DataContext _dataContext;

    public ChatService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public string DrawUser()
    {
        Random rng = new Random();
        var users = _dataContext.Users
            .Where(x => x.Status == true)
            .ToList();
        var randUser = rng.Next(users.Count()) + 1;
        var user = users.Single(u => u.Id == randUser);

        var room = new Room() {};
        
        _dataContext.Rooms.Add(room);
        _dataContext.SaveChanges();
        return room.RoomId;
    }
}