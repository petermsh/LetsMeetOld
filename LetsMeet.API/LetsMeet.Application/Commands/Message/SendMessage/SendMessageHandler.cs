using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Message;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Application.Hubs;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.Application.Commands.Message.SendMessage;

internal sealed class SendMessageHandler : ICommandHandler<SendMessageCommand,string>
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SendMessageHandler(IHubContext<ChatHub> hubContext, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IMessageRepository messageRepository)
    {
        _hubContext = hubContext;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _messageRepository = messageRepository;
    }

    public async Task<string> HandleAsync(SendMessageCommand command)
    {
        var user = await _userRepository.GetByUsernameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

        if (user is null)
            throw new UserNotFoundException("");
        
        var message = new Core.Domain.Entities.Message
        {
            Content = command.Content,
            RoomId = command.RoomId,
            SenderUserName = user.UserName
        };
        
        var sendMessage = new SendMessageDto
        {
            Content = message.Content,
            Date = message.MessageSent,
            From = message.SenderUserName,
            RoomId = message.RoomId
        };
        
        await _hubContext.Clients.Group(sendMessage.RoomId).SendAsync("ReceiveMessage", sendMessage);
        await _messageRepository.AddAsync(message);
        user.CountMessage();
        return "success";
    }
}