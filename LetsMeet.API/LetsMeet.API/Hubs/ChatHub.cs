using System.Runtime.CompilerServices;
using AutoMapper;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Hubs;

public class ChatHub : Hub
{
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(DataContext dataContext, IUserInfoProvider userInfoProvider, IMapper mapper, ILogger<ChatHub> logger)
    {
        _dataContext = dataContext;
        _userInfoProvider = userInfoProvider;
        _mapper = mapper;
        _logger = logger;
    }
    
    // public override Task OnDisconnectedAsync(Exception? exception)
    // {
    //     var userConnection = _dataContext.UserConnections
    //         .FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
    //
    //     if (userConnection is not null)
    //     {
    //         _dataContext.Remove(userConnection);
    //         Clients.Group(userConnection.RoomId)
    //             .SendAsync("ReceiveMessage", string.Empty, $"{userConnection.User.Nick} has left");
    //     }
    //
    //     return base.OnDisconnectedAsync(exception);
    // }

    // public async Task SendMessage(string message)
    // {
    //     var userConnection = _dataContext.UserConnections
    //         .FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
    //
    //     if (userConnection is not null)
    //     {
    //         await Clients.Group(userConnection.RoomId).SendAsync("ReceiveMessage", userConnection.User.Nick, message);
    //     }
    // }
    
    // public Task JoinRoom(string roomId)
    // {
    //     // var user = _userInfoProvider.CurrentUser;
    //
    //     // var room = _dataContext.Rooms.FirstOrDefault(x => x.RoomId == roomId);
    //     //
    //     // if (room is null)
    //     // {
    //     //     var newRoom = new Room()
    //     //     {
    //     //         RoomId = roomId
    //     //     };
    //     // }
    //     // else
    //     // {
    //     //     var messages = _dataContext.Messages
    //     //         .Where(x => x.RoomId == roomId)
    //     //         .Select(x=>_mapper.Map<MessageListDTO>(x))
    //     //         .OrderBy(x => x.Date)
    //     //         .ToList();
    //     //     await Clients.Group(roomId).SendAsync("ReceiveMessages", messages);
    //     // }
    //     
    //     // await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    //     // var userConnection = new UserConnection()
    //     // {
    //     //     ConnectionId = Context.ConnectionId,
    //     //     UserId = user.Id,
    //     //     RoomId = roomId
    //     // };
    //     //
    //     // _dataContext.UserConnections.Add(userConnection);
    //     // _dataContext.SaveChanges();
    //     //
    //     // await Clients.Group(roomId).SendAsync("ReceiveMessage", string.Empty, $"{userConnection.User.Nick} has joined");
    //
    //     return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    // }
    
    public void JoinGroup(string groupName)
    {
        _logger.LogInformation("JoinGroup executing...");
        
        this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
    }

    public async Task LeaveRoom(string roomId)
    {
        _logger.LogInformation("LeaveGroup executing...");
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

}