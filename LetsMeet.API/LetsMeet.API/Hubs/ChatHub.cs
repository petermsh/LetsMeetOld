using System.Runtime.CompilerServices;
using System.Security.Claims;
using AutoMapper;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using LetsMeet.API.Infrastructure;
using LetsMeet.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace LetsMeet.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _dataContext;
    private readonly IUserInfoProvider _userInfoProvider;
    public ChatHub(UserManager<User> userManager, DataContext dataContext, IUserInfoProvider userInfoProvider)
    {
        _userManager = userManager;
        _dataContext = dataContext;
        _userInfoProvider = userInfoProvider;
    }
    
    public override async Task OnConnectedAsync()
    {
        var userName = Context.User.Identity.Name;
        var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);

        user.Status = true;
        _dataContext.Users.Update(user);
        _dataContext.SaveChanges();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userName = Context.User.Identity.Name;
        var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);

        user.Status = false;
        _dataContext.Users.Update(user);
        _dataContext.SaveChanges();
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var message = new Message
        {
            Content = "eloelo",
            RoomId = "5fe0f2e1-f765-4543-92b0-f5a803d99c59",
            SenderUserName = _userInfoProvider.Name
        };

        await Clients.Group("5fe0f2e1-f765-4543-92b0-f5a803d99c59").SendAsync("ReceiveMessage", message);
        await _dataContext.Messages.AddAsync(message);
        await _dataContext.SaveChangesAsync();
    }
    
    
    public async Task CreateRoom(User user)
    {
        var currentUserName = Context.User.Identity.Name;
        
        var room = new Room
        {
        };
        await _dataContext.Rooms.AddAsync(room);
        await _dataContext.SaveChangesAsync();
        
        var currCon = new Connection
        {
            UserId = _userInfoProvider.Id,
            RoomId = room.RoomId
        };
        await _dataContext.Connections.AddAsync(currCon);
        await _dataContext.SaveChangesAsync();

         var drawCon = new Connection
         {
             UserId = user.Id,
             RoomId = room.RoomId
         };
        
        await  _dataContext.Connections.AddAsync(drawCon);
        await _dataContext.SaveChangesAsync();
    }
    
    public async Task JoinRoom(Room room)
    {
        var messages = _dataContext.Messages.Where(x => x.RoomId == room.RoomId)
            .Select(query=> new SingleMessageDto
            {
                From = query.SenderUserName,
                Content = query.Content,
                Date = query.CreatedAt
            }).ToList();

        await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);
        await Clients.Group(room.RoomId).SendAsync("ReceiveMessage", $"{Context.User.Identity.Name} has joined");
        await Clients.Group(room.RoomId).SendAsync("ReceiveMessage", messages);
    }

    public async Task LeaveRoom(Room room)
    {
        await Clients.Group(room.RoomId).SendAsync("ReceiveMessage", $"{Context.User.Identity.Name} has left.");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.RoomId);
    }
}
    