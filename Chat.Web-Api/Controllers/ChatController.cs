using System.Text;
using Chat.Bll;
using Chat.Bll.Commands.Chats;
using Chat.Bll.Commands.Messages;
using Chat.Bll.Queries.Chats;
using Chat.Bll.Queries.Users;
using Chat.Bll.Requests;
using Chat.Dal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Web_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private IMediator _mediator { get; }
    private ChatContext _context { get; }
    private IHubContext<ChatHub> _chatHub { get; }


    public ChatController(IMediator mediator, ChatContext dbContext,IHubContext<ChatHub> chatHub)
    {
        _mediator = mediator;
        _context = dbContext;
        _chatHub = chatHub;
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

    [HttpPost("ArchiveChat")]
    public async Task<IActionResult> ArchiveChat(ArchiveChatRequest request)
    {
        var currentUserId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "GUID")?.Value;
        var archiveChatCommandResult = await _mediator.Send(new ArchiveChatCommand(currentUserId,request.FriendId));
        if (archiveChatCommandResult.bytes == null)
        {
            return BadRequest(new { data = "Что-то пошло не так" });
        }

        var fileName = new StringBuilder();
        fileName.Append("archiveChat");
        fileName.Append(Guid.NewGuid().ToString().Substring(0,10));
        fileName.Append(".txt");
        return File(archiveChatCommandResult.bytes,"plain/text",fileName.ToString());
    }

    // [AllowAnonymous]
    // [HttpGet("AddMessage")]
    // public async Task<IActionResult> AddMessage()
    // {
    //     var addMessageCommandResult =
    //         await _mediator.Send(new AddMessageCommand(new Guid("d64c734c-1a2d-4625-b2f9-b8d4db3f20a9"), new Guid("57f0538b-5785-4721-a851-8d4c7c9e2d5b"), "новое сообщение", _chatHub.Clients));
    //     return Ok();
    // }
}