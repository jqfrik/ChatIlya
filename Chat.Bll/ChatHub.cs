using Chat.Bll.Commands.Messages;
using Chat.Dal;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bll;

public class ChatHub : Hub
{
    public ChatContext DbContext { get; }
    public IMediator Mediator { get; }

    public ChatHub(ChatContext dbContext, IMediator mediator)
    {
        DbContext = dbContext;
        Mediator = mediator;
    }
    public override async Task OnConnectedAsync()
    {
        // var connectionId = Context.ConnectionId;
        // var contextName =  Context.User.Claims.FirstOrDefault(cl => cl.Type.Contains("claims/name")).Value;
        // var contextId = Context.User.Claims.FirstOrDefault(cl => cl.Type == "GUID")?.Value;
        // var user = DbContext.Users.FirstOrDefault(user => user.Id == new Guid(contextId));
        // if(user.ConnectionId != connectionId)
        //     user.ConnectionId = connectionId;
        // await DbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task UpdateSignalId(string userId)
    {
        var connectionId = Context.ConnectionId;
        var user = DbContext.Users.FirstOrDefault(user => user.Id == new Guid(userId));
        if(user.ConnectionId != connectionId)
            user.ConnectionId = connectionId;
        await DbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SendMessageClient(string message,string userId,string friendId)
    {
        var user = Context.User;
        var connectionId = Context.ConnectionId;
        //Проверить как работает приведение типов
        AddMessageCommandResult addMessageCommandResult = await Mediator.Send(new AddMessageCommand(new Guid(userId), new Guid(friendId), message));
    }

    public async Task RemoveMessageClient(string messageId, string chatId)
    {
        var user = Context.User;
        var connectionId = Context.ConnectionId;
        var userContextId = Context.User.Claims.FirstOrDefault(cl => cl.Type == "GUID")?.Value;

        var removeMessageCommandResult = await Mediator.Send(new RemoveMessageCommand(userContextId, chatId, messageId));
    }
}