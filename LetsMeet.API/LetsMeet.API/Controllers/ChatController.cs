using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Enums;
using LetsMeet.API.Hubs;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : Controller
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    [HttpGet("draw")]
    public IActionResult DrawUser(bool isUniversity, bool isCity, [FromQuery] Gender gender)
    {
        var userName = _chatService.DrawUser(isUniversity, isCity, (int)gender);
        return Ok(userName);
    }


}