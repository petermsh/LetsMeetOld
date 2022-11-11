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
    
    public DrawUserDto DrawUser()
    {
         Random rng = new Random();
         var users = _dataContext
              .Users
              .Where(x=>x.Id != _userInfoProvider.Id && x.Status == true).ToList();
         var randUser = rng.Next(users.Count());
         var user = users[randUser];
         var drawUser = new DrawUserDto
         {
             Id = user.Id,
             Nick = user.UserName
         };
        
        return drawUser;
    }
}