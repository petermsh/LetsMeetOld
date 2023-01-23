using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Enums;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

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
    
    public CreatedRoomDto DrawUser(bool isUniversity, bool isCity, int gender)
    {
        var currentUser = _userInfoProvider.CurrentUser;
        var secondUser = new User() {};
        var users = new List<User> {} ;
        int counter = 0;

        Random rng = new Random();
        if (gender == 0)
        {
            users = _dataContext
                .Users
                .Where(x => x.Id != _userInfoProvider.Id)
                .Where(x => x.Status == true)
                .ToList();
        }
        else
        {
            users = _dataContext
                .Users
                .Where(x => x.Id != _userInfoProvider.Id)
                .Where(x => x.Status == true)
                .Where(x=>x.Gender == (Gender)gender)
                .ToList();
        }
        

        if (isCity)
            users = users.Where(x => x.City == currentUser.City).ToList();

        if (isUniversity)
            users = users.Where(x => x.University == currentUser.University).ToList();

        users = users.OrderByDescending(x => x.MessageCount)
            .Take(5)
            .ToList();

        if (users.Count == 0)
             throw new UsersNotFoundException();
        do
        {
            var randUser = rng.Next(users.Count());
            var user = users[randUser];
            var drawUser = new DrawUserDto
            {
                Id = user.Id,
                UserName = user.UserName
            };

            secondUser = _dataContext.Users.FirstOrDefault(x => x.Id == drawUser.Id);
            counter++;

        } while (_dataContext.Rooms
                     .FirstOrDefault(r => r.Users.Any(u => u.Id == secondUser.Id)
                                          && r.Users.Any(u => u.Id == currentUser.Id)) is not null
                 && counter < 10);

        if (counter == 10)
            throw new UsersNotFoundException();
        //return drawUser;
        
        //temporary

        var room = new Room
        {
            Users = new List<User>() { secondUser, currentUser}
        };
        _dataContext.Rooms.Add(room);
        _dataContext.SaveChanges();

        var message = new Message
        {
            Content = "Utworzono konwersację",
            RoomId = room.RoomId,
            SenderUserName = "LetsMeet"
        };

        _dataContext.Messages.Add(message);
        _dataContext.SaveChanges();
        
        return new CreatedRoomDto()
        {
            Users = new List<string>() { currentUser.UserName, secondUser.UserName},
            roomId = room.RoomId
        };
    }
}