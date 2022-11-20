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
using LetsMeet.API.Exceptions;
using LetsMeet.API.Infrastructure;
using LetsMeet.API.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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
            Content = createMessageDto.Content,
            RoomId = createMessageDto.RoomId,
            SenderUserName = _userInfoProvider.Name
        };

        var singleMessage = new SingleMessageDto
        {
            Content = message.Content,
            Date = message.MessageSent,
            From = message.SenderUserName,
            FromUser = _userInfoProvider.Name == message.SenderUserName
        };

        await Clients.Group(createMessageDto.RoomId).SendAsync("ReceiveMessage", singleMessage);
        await _dataContext.Messages.AddAsync(message);
        await _dataContext.SaveChangesAsync();
    }

    public async Task CreateRoom(User user)
    {
        var currentUserName = Context.User.Identity.Name;
        
        var room = new Room
        {
        };

        var secondUser = _dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
        room.Users.Add(secondUser);
        room.Users.Add(_userInfoProvider.CurrentUser);
        await _dataContext.Rooms.AddAsync(room);
        await _dataContext.SaveChangesAsync();
        
    }
    
    public async Task JoinRoom(Room room)
    {
        var rooms = _dataContext.Rooms.FirstOrDefault(x => x.RoomId == room.RoomId);

        if (rooms is null)
        {
            throw new RoomNotFoundException();
        }
        
        var messages = _dataContext.Messages.Where(x => x.RoomId == room.RoomId)
            .Select(query=> new SingleMessageDto
            {
                From = query.SenderUserName,
                Content = query.Content,
                Date = query.MessageSent,
                FromUser = _userInfoProvider.Name == query.SenderUserName
            }).OrderByDescending(m=>m.Date).ToList();
        

        if (rooms.isLocked)
        {
            throw new RoomIsLockedException();
        }
        
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", messages);
        await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);
        await Clients.Group(room.RoomId).SendAsync("ReceiveMessage", $"{Context.User.Identity.Name} has joined");
    }

    public async Task LeaveRoom(Room room)
    {
        await Clients.Group(room.RoomId).SendAsync("ReceiveMessage", $"{Context.User.Identity.Name} has left.");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.RoomId);
    }

    public async Task GetRoomsList()
    {
        var rooms = _dataContext.Rooms
            .Where(r=>r.isLocked == false && r.Users.All(x=>x.Id == _userInfoProvider.Id))
            .Select(query=>new RoomInfoDto
            {
                RoomId = query.RoomId,
                RoomName = query.RoomName,
                LastMessage = query.Messages.OrderByDescending(x=>x.CreatedAt).FirstOrDefault().Content
            }).ToList();

        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", rooms);
    }
}
    