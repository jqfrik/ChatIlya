using Chat.Bll.Commands.Chats;
using Chat.Bll.MailService;
using Chat.Bll.Queries.Chats;
using Chat.Bll.Queries.Users;
using Chat.Bll.Requests;
using Chat.Dal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private IMediator _mediator { get; }
    private ChatContext _context { get; }

    public ChatController(IMediator mediator, ChatContext dbContext)
    {
        _mediator = mediator;
        _context = dbContext;
    }
    
    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateChat(CreateChatWithFriendRequest request)
    {
        CreateChatWithFriendCommandResult commandResult = await _mediator.Send(new CreateChatWithFriendCommand(request.CurrentUserId,request.FriendUserId));
        return Ok(new {data = commandResult.ChatId});
    }
    
    [Authorize]
    [HttpPost("CheckChatWithFriend")]
    public async Task<IActionResult> CheckChatWithCurrentFriend(CheckChatWithCurrentFriendRequest request)
    {
        var checkChatWithCurrentFriendResult = await _mediator.Send(
            new CheckChatWithCurrentFriendCommand(request.CurrentUserId, request.FriendUserId, _context));
        if(checkChatWithCurrentFriendResult.Success)
            return Ok(new {success = true});
        return Ok(new {success = false});
    }

    [Authorize]
    [HttpGet("GetChatById/{chatId}")]
    public async Task<IActionResult> GetChatById([FromRoute]string chatId)
    {
        var currentUserId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "GUID")?.Value;
        var getChatByIdCommandResult = await _mediator.Send(new GetChatByChatIdCommand(chatId, currentUserId, _context));
        return Ok(new {data = getChatByIdCommandResult.Chat});
    } 
}