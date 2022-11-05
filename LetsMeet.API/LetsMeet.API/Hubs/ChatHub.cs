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
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly PresenceTracker _tracker;
    private readonly IHubContext<PresenceHub> _presenceHub;
    private readonly IUserIdProvider _userIdProvider;

    public ChatHub(DataContext dataContext, IUserInfoProvider userInfoProvider, IMapper mapper, ILogger<ChatHub> logger, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IUserService userService, ChatRegistry chatRegistry, IMessageService messageService, IUnitOfWork unitOfWork, PresenceTracker tracker, IHubContext<PresenceHub> presenceHub, IUserIdProvider userIdProvider)
    {
        _mapper = mapper;
        _userService = userService;
        _messageService = messageService;
        _unitOfWork = unitOfWork;
        _tracker = tracker;
        _presenceHub = presenceHub;
        _userIdProvider = userIdProvider;
    }
    
    public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var fff = httpContext.User.Identity.Name;
            var ggg = httpContext.Connection.Id;
            // var otherUser = httpContext.Request.Query["user"].ToString();
            // var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
            // await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            // var group = await AddToGroup(groupName);
            // await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
            //
            // var messages = await _messageService.
            //     GetMessageThread(Context.User.GetUserName(), otherUser);
            //
            // if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();
            //
            // await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
            var cth = Context.User.Identity.Name;

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // var group = await RemoveFromMessageGroup();
            // await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            // await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            //var userid = _userIdProvider.GetUserId()
            
            var username = Context.User.GetUserName();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            var sender = await _userService.GetUserByUsernameAsync(username);
            var recipient = await _userService.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _messageService.GetMessageGroup(groupName);

            // if (group.Connections.Any(x => x.UserName == recipient.UserName))
            // {
            //     message.DateRead = DateTime.UtcNow;
            // }
            // else
            // {
            //     var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
            //     if (connections != null)
            //     {
            //         await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
            //             new { username = sender.UserName });
            //     }
            // }

            _messageService.AddMessage(message);

            if (await _unitOfWork.Complete())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _messageService.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUserName());

            if (group == null)
            {
                group = new Group(groupName);
                _messageService.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageService.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _messageService.RemoveConnection(connection);
            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
}
    