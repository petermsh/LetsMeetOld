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
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == _userInfoProvider.Name);

        if (user is null)
            throw new UserNotFoundException("");
        
        var message = new Message
        {
            Content = createMessageDto.Content,
            RoomId = createMessageDto.Room,
            SenderUserName = _userInfoProvider.Name
        };

        var singleMessage = new SingleMessageDto
        {
            Content = message.Content,
            Date = message.MessageSent,
            From = message.SenderUserName,
            FromUser = _userInfoProvider.Name == message.SenderUserName
        };
        

        await Clients.Group(createMessageDto.Room).SendAsync("ReceiveMessage", singleMessage);
        await _dataContext.Messages.AddAsync(message);
        user.MessageCount++;
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task CreateRoom(User user)
    {
        var currentUserName = Context.User.Identity.Name;
        var secondUser = _dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
        
        var room = new Room
        {
            Users = new List<User>() { secondUser, _userInfoProvider.CurrentUser}
        };
        
        await _dataContext.Rooms.AddAsync(room);
        await _dataContext.SaveChangesAsync();
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", room.RoomId);
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

    public async Task<List<RoomInfoDto>> GetRoomsList()
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == Context.User.Identity.Name);

        if (user is null)
            throw new UserNotFoundException("");
        
        var rooms = await _dataContext.Rooms
            .Where(r=>r.isLocked == false)
            .Where(r=>r.Users.Any(x=>x.Id == user.Id))
            .Select(query=>new RoomInfoDto
            {
                RoomId = query.RoomId,
                RoomName = query.RoomName,
                LastMessage = query.Messages.OrderByDescending(x=>x.CreatedAt).FirstOrDefault().Content
            }).ToListAsync();

        if (rooms is null)
            throw new RoomsNotFoundException();
        
        await Clients.Client(user.Id).SendAsync("ReceiveMessage", rooms);
        return rooms;
    }
}
    