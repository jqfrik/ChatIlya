using Chat.Dal;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bll.Commands.Messages.AddMessage;

public class AddMessageCommand : IRequest<AddMessageCommandResult>
{
    public Guid? UserId { get; }
    public Guid? FriendId { get; }
    public string Message { get; }
    public ChatContext Context { get; }
    public HubCallerContext HubContext { get; }
    public IHubCallerClients Clients { get; }

    public AddMessageCommand(Guid? userId, Guid? friendId, string message, ChatContext context, HubCallerContext hubContext, IHubCallerClients clients)
    {
        UserId = userId;
        FriendId = friendId;
        Message = message;
        Context = context;
        HubContext = hubContext;
        Clients = clients;
    }
}