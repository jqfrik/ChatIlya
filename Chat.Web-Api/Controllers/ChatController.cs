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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

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

    [Authorize]
    [HttpPost("ArchiveChat")]
    public async Task<IActionResult> ArchiveChat(ArchiveChatRequest request)
    {
        var currentUserId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "GUID")?.Value;
        var archiveChatCommandResult = await _mediator.Send(new ArchiveChatCommand(currentUserId,request.ChatId));
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
    
    [HttpPost("GetMessagesByChatId")]
    public async Task<IActionResult> GetMessagesByChatId(GetMessagesByChatIdRequest request)
    {
        var getMessagesByChatIdResult = await _mediator.Send(new GetMessagesByChatIdCommand(request.ChatId));
        return Ok(getMessagesByChatIdResult.Messages);
    }

    [HttpGet("GetAllChatsByUserId/{userId}")]
    public async Task<IActionResult> GetAllChatsByUserId([FromRoute] string userId)
    {
        var allChatsCommandResult = await _mediator.Send(new GetAllChatsByUserIdCommand(userId));
        return Ok(allChatsCommandResult.Chats);
    }

    [Authorize]
    [HttpPost("SendAttachments")]
    public async Task<IActionResult> SendAttachments()
    {
        var reader = new MultipartReader("", Request.Body);
        var section = await reader.ReadNextSectionAsync();
        while (section != null)
        {
            var stream = section.Body;
            var contentDisposition = section.ContentDisposition;
            if (contentDisposition.Contains("file"))
            {
                var fileName = Guid.NewGuid().ToString();
                using var newStream = new FileStream(fileName,FileMode.OpenOrCreate);
                await stream.CopyToAsync(newStream);
            } 
            section = await reader.ReadNextSectionAsync();
        }

        return Ok();
    }
}