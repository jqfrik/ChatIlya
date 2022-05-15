using Chat.Bll.Commands.Chats.CreateChatWithFriend;
using Chat.Bll.Dtos;
using Chat.Dal;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    public IMediator Mediator { get; }
    public ChatContext DbContext { get; }

    public ChatController(IMediator mediator, ChatContext dbContext)
    {
        Mediator = mediator;
        DbContext = dbContext;
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateChat(CreateChatWithFriendRequest request)
    {
        CreateChatWithFriendCommandResult commandResult = await Mediator.Send(new CreateChatWithFriendCommand(request.MainUser,request.SecondaryUser,DbContext));

        if (!commandResult.Success)
            return BadRequest(new { success = false });
        return Ok(new { success = true });
    }
}