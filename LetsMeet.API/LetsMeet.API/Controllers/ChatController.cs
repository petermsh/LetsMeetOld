using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Hubs;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Controllers;

[Route("api/chat")]
public class ChatController : Controller
{
    private readonly DataContext _dataContext;
    private readonly Hub<ChatHub> _hubContext;
    private readonly IChatService _chatService;
    private readonly IUserInfoProvider _userInfoProvider;

    public ChatController(Hub<ChatHub> hubContext, DataContext dataContext, IChatService chatService, IUserInfoProvider userInfoProvider)
    {
        _hubContext = hubContext;
        _dataContext = dataContext;
        _chatService = chatService;
        _userInfoProvider = userInfoProvider;
    }

    [Route("find")]
    [HttpGet]
    [Authorize]
    public string? DrawUser()
    {
        //return _chatService.DrawUser();
        return _userInfoProvider.Name;
    }
    


}