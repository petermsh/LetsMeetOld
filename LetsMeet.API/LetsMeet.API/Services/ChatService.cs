using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Interfaces;

namespace LetsMeet.API.Services;

public class ChatService : IChatService
{
    private readonly DataContext _dataContext;
    private readonly IUserInfoProvider _userInfoProvider;

    public ChatService(DataContext dataContext, IUserInfoProvider userInfoProvider)
    {
        _dataContext = dataContext;
        _userInfoProvider = userInfoProvider;
    }
    
    public FindUserDto DrawUser()
    {
        Random rng = new Random();
        var users = _dataContext.Users
            .Where(x => x.Status == true && x.Id != _userInfoProvider.CurrentUser.Id)
            .ToList();
        var randUser = rng.Next(users.Count()) + 1;
        var user = users.Single(u => u.Id == randUser);

        var room = new Room() {};
        _dataContext.Rooms.Add(room);
        _dataContext.SaveChanges();
        
        var firstConnection = new UserConnection()
        {
            UserId = _userInfoProvider.CurrentUser.Id,
            RoomId = room.RoomId,
        };

        var secondConnection = new UserConnection()
        {
            UserId = user.Id,
            RoomId = room.RoomId
        };
        
        _dataContext.UserConnections.Add(firstConnection);
        _dataContext.UserConnections.Add(secondConnection);
        _dataContext.SaveChanges();

        var findUser = new FindUserDto()
        {
            Nick = user.Nick,
            RoomId = room.RoomId,
            CreatedDateTime = room.CreatedAt,
        };
        
        return findUser;
    }
}