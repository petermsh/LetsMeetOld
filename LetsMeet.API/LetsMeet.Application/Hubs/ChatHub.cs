using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Commands.Message.SendMessage;
using LetsMeet.Application.DTO.Message;
using LetsMeet.Application.DTO.Room;
using LetsMeet.Application.Exceptions.Room;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Application.Queries.Messages.GetMessagesFromRoom;
using LetsMeet.Application.Queries.Room.GetRooms;
using LetsMeet.Core.Domain.Entities;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.Application.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly UserManager<User> _userManager;
    private readonly IRoomRepository _roomRepository;
    private readonly ICommandHandler<SendMessageCommand, string> _sendMessageHandler;
    private readonly IQueryHandler<GetMessagesFromRoom, List<MessageDetailsDto>> _getMessagesHandler;
    private readonly IQueryHandler<GetRooms, List<RoomsDto>> _getRoomsHandler;

    public ChatHub(UserManager<User> userManager, ICommandHandler<SendMessageCommand, string> sendMessageHandler, IQueryHandler<GetMessagesFromRoom, List<MessageDetailsDto>> getMessagesHandler, IRoomRepository roomRepository, IQueryHandler<GetRooms, List<RoomsDto>> getRoomsHandler)
    {
        _userManager = userManager;
        _sendMessageHandler = sendMessageHandler;
        _getMessagesHandler = getMessagesHandler;
        _roomRepository = roomRepository;
        _getRoomsHandler = getRoomsHandler;
    }
    
    public override async Task OnConnectedAsync()
    {
        var userName = Context.User.Identity.Name;
        var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);

        if (user is null)
            throw new UserNotFoundException("");
        user.ChangeStatus(true);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userName = Context.User.Identity.Name;
        var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);

        if (user is null)
            throw new UserNotFoundException("");
        user.ChangeStatus(false);
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var userName = Context.User.Identity.Name;
        var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);
        
        var command = new SendMessageCommand(createMessageDto.Room, createMessageDto.Content);
        await _sendMessageHandler.HandleAsync(command);

        var room = await _roomRepository.GetAsync(createMessageDto.Room);

        var currentUserRooms = await _getRoomsHandler.HandleAsync(new GetRooms(user.Id));
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveRooms", currentUserRooms);
        
    }
    
    public async Task JoinRoom(Room room)
    {
        var rooms = await _roomRepository.GetAsync(room.RoomId);
    
        if (rooms is null)
        {
            throw new RoomNotFoundException();
        }

        var query = new GetMessagesFromRoom(room.RoomId);
        var messages = await _getMessagesHandler.HandleAsync(query);
    
        if (rooms.IsLocked)
        {
            throw new RoomLockedException();
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
        var userName = Context.User.Identity.Name;
        var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);
        
        if (user is null)
            throw new UserNotFoundException("");

        var rooms = await _getRoomsHandler.HandleAsync(new GetRooms(user.Id))
                    ?? throw new RoomsNotFoundException();

        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveRooms", rooms);
    }
    
    // public async Task SendMessage(CreateMessageDto createMessageDto)
    // {
    //     var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == Context.User.Identity.Name);
    //
    //
    //     if (user is null)
    //         throw new UserNotFoundException("");
    //     
    //     var message = new Message
    //     {
    //         Content = createMessageDto.Content,
    //         RoomId = createMessageDto.Room,
    //         SenderUserName = user.UserName
    //     };
    //
    //     var singleMessage = new SingleMessageAddDto
    //     {
    //         Content = message.Content,
    //         Date = message.MessageSent,
    //         From = message.SenderUserName,
    //         RoomId = createMessageDto.Room
    //     };
    //
    //     await Clients.Group(createMessageDto.Room).SendAsync("ReceiveMessage", singleMessage);
    //     await _dataContext.Messages.AddAsync(message);
    //     user.MessageCount++;
    //     _dataContext.Users.Update(user);
    //     await _dataContext.SaveChangesAsync();
    //
    // }
    //
    // public async Task UpdateMessage(UpdateMessageDto updateMessageDto)
    // {
    //     var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == Context.User.Identity.Name);
    //
    //
    //     if (user is null)
    //         throw new UserNotFoundException("");
    //
    //     var message = await _dataContext.Messages.FirstOrDefaultAsync(x => x.Id == updateMessageDto.MessageId);
    //     if (message is null)
    //         throw new MessageNotFoundException();
    //
    //     message.Content = updateMessageDto.Content;
    //     
    //     _dataContext.Messages.Update(message);
    //     await _dataContext.SaveChangesAsync();
    //     
    //     var messages = _dataContext.Messages.Where(x => x.RoomId == message.RoomId)
    //         .Select(query=> new SingleMessageToListDto
    //         {
    //             From = query.SenderUserName,
    //             Content = query.Content,
    //             Date = query.MessageSent,
    //             FromUser = _userInfoProvider.Name == query.SenderUserName
    //         }).OrderBy(m=>m.Date).ToList();
    //     
    //     await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", messages);
    // }
    //
    // public async Task CreateRoom(User user)
    // {
    //     var currentUserName = Context.User.Identity.Name;
    //     var secondUser = _dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
    //     
    //     var room = new Room
    //     {
    //         Users = new List<User>() { secondUser, _userInfoProvider.CurrentUser}
    //     };
    //     
    //     await _dataContext.Rooms.AddAsync(room);
    //     await _dataContext.SaveChangesAsync();
    //     await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", room.RoomId);
    // }
    //
    // public async Task JoinRoom(Room room)
    // {
    //     var rooms = _dataContext.Rooms.FirstOrDefault(x => x.RoomId == room.RoomId);
    //
    //     if (rooms is null)
    //     {
    //         throw new RoomNotFoundException();
    //     }
    //     
    //     var messages = _dataContext.Messages.Where(x => x.RoomId == room.RoomId)
    //         .Select(query=> new SingleMessageToListDto
    //         {
    //             From = query.SenderUserName,
    //             Content = query.Content,
    //             Date = query.MessageSent,
    //             FromUser = _userInfoProvider.Name == query.SenderUserName
    //         }).OrderBy(m=>m.Date).ToList();
    //     
    //
    //     if (rooms.isLocked)
    //     {
    //         throw new RoomIsLockedException();
    //     }
    //     
    //     await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", messages);
    //     await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);
    //     await Clients.Group(room.RoomId).SendAsync("ReceiveMessage", $"{Context.User.Identity.Name} has joined");
    // }
    //
    //
    // public async Task<List<RoomInfoDto>> GetRoomsList()
    // {
    //     var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == Context.User.Identity.Name);
    //
    //     if (user is null)
    //         throw new UserNotFoundException("");
    //     
    //     var rooms = await _dataContext.Rooms
    //         .Where(r=>r.isLocked == false)
    //         .Where(r=>r.Users.Any(x=>x.Id == user.Id))
    //         .Select(query=>new RoomInfoDto
    //         {
    //             RoomId = query.RoomId,
    //             RoomName = query.Users.Where(x=>x.UserName != user.UserName).Select(x=>x.UserName).FirstOrDefault(),
    //             LastMessage = query.Messages.OrderByDescending(x=>x.CreatedAt).FirstOrDefault().Content
    //         }).ToListAsync();
    //
    //     if (rooms is null)
    //         throw new RoomsNotFoundException();
    //     
    //     await Clients.Client(Context.ConnectionId).SendAsync("ReceiveRooms", rooms);
    //     return rooms;
    // }
}