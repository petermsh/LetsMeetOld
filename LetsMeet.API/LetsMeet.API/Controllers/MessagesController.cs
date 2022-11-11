using AutoMapper;
using LetsMeet.API.DTO;
using LetsMeet.API.Helper;
using LetsMeet.API.Infrastructure;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.API.Controllers;

[Authorize]
public class MessagesController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageService _messageService;

    public MessagesController(IMapper mapper, IUnitOfWork unitOfWork, IMessageService messageService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _messageService = messageService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]
        MessageParams messageParams)
    {
        messageParams.Username = User.GetUserName();

        var messages = await _messageService.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
            messages.TotalCount, messages.TotalPages);

        return messages;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUserName();

        var message = await _messageService.GetMessage(id);

        if (message.Sender.UserName != username && message.Recipient.UserName != username)
            return Unauthorized();

        if (message.Sender.UserName == username) message.SenderDeleted = true;

        if (message.Recipient.UserName == username) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted)
            _messageService.DeleteMessage(message);

        if (await _unitOfWork.Complete()) return Ok();

        return BadRequest("Problem deleting the message");
    }
}