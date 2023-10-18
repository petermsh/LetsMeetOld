using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Room;
using LetsMeet.Application.DTO.User;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Application.Hubs;
using LetsMeet.Core.Domain.Enums;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace LetsMeet.Application.Commands.Room.CreateRoom;

internal sealed class CreateRoomHandler : ICommandHandler<CreateRoomCommand, CreatedRoomDto>
{
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IHubContext<ChatHub> _hubContext;

    public CreateRoomHandler(IUserInfoProvider userInfoProvider, IUserRepository userRepository, IRoomRepository roomRepository, IMessageRepository messageRepository, IHubContext<ChatHub> hubContext)
    {
        _userInfoProvider = userInfoProvider;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _messageRepository = messageRepository;
        _hubContext = hubContext;
    }

    public async Task<CreatedRoomDto> HandleAsync(CreateRoomCommand command)
    {
        Random rng = new Random();
        var secondUser = new Core.Domain.Entities.User() {};
        var users = new List<Core.Domain.Entities.User> {} ;
        int counter = 0; 
        var existingRoom = new Core.Domain.Entities.Room();

        var currentUser = await _userRepository.GetByIdAsync(new Guid(_userInfoProvider.UserId));
        
        if (currentUser is null)
                 throw new UserNotFoundException("");

        if (command.Gender == 0)
        {
            users = await _userRepository.GetUsersAsync(x => x.Id != currentUser.Id && x.Status == true);
        }
        else
        {
            users = await _userRepository.GetUsersAsync(x =>
                x.Id != currentUser.Id && x.Status == true && x.Gender == command.Gender);
        }

        if (command.City != null)
            users = users.Where(x => x.City == command.City).ToList();

        if (command.University != null)
            users = users.Where(x => x.University == command.University).ToList();
        
        if (command.Major != null)
            users = users.Where(x => x.Major == command.Major).ToList();

        users = users.OrderByDescending(x => x.MessageCount)
            .Take(10)
            .ToList();
        
        if (users.Count == 0)
            throw new UsersNotFoundException();

        do
        {
            var randUser = rng.Next(users.Count());
            secondUser = users[randUser];

            existingRoom = await _roomRepository.GetRoomWhereUsersAsync(secondUser.Id, currentUser.Id);
            if (existingRoom == null) continue;
            users.Remove(secondUser);
            secondUser = null;

        } while (existingRoom != null && users != null);

        if (secondUser is null)
            throw new UsersNotFoundException();
        
        var room = new Core.Domain.Entities.Room
        {
            Users = new List<Core.Domain.Entities.User>() { secondUser, currentUser}
        };

        await _roomRepository.AddAsync(room);
        
        var message = new Core.Domain.Entities.Message
        {
            Content = "Utworzono konwersację",
            RoomId = room.RoomId,
            SenderUserName = "LetsMeet"
        };

        await _messageRepository.AddAsync(message);
        await _hubContext.Groups.AddToGroupAsync(command.ConnectionId, room.RoomId);
        await _hubContext.Clients.Group(room.RoomId).SendAsync("ReceiveMessage", message.Content);
        
        return new CreatedRoomDto()
        {
            Users = new List<string>() { currentUser.UserName, secondUser.UserName},
            RoomId = room.RoomId
        };
    }
}