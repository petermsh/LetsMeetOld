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
    
    public string? DrawUser()
    {
        // Random rng = new Random();
        // var users = _dataContext
        //     .Users.ToList();
        // var randUser = rng.Next(users.Count()) + 1;
        // var user = users
        //     .Where(x=>x.Id==randUser)
        //     .ToString();

        // var room = new Room() {};
        // _dataContext.Rooms.Add(room);
        // _dataContext.SaveChanges();
        
        
        // var findUser = new FindUserDto()
        // {
        //     Nick = user.Nick,
        //     RoomId = room.RoomId,
        //     CreatedDateTime = room.CreatedAt,
        // };
        
        return "g";
    }
}