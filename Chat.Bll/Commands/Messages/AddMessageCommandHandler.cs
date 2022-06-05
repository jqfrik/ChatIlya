using Chat.Dal;
using Chat.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Messages;

public record AddMessageCommand(Guid? UserId, Guid? FriendId, string Message,HubCallerContext HubContext, IHubCallerClients Clients) : IRequest<AddMessageCommandResult>;

public record AddMessageCommandResult(bool Success);
public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand,AddMessageCommandResult>
{ 
    private ChatContext _context { get; }

    public AddMessageCommandHandler(ChatContext context)
    {
        _context = context;
    }
    public async  Task<AddMessageCommandResult> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var chats = _context.Chats
                .Include(x => x.Users)
                .Include(x => x.Messages);
            var users = _context.Users;
            var currentUser = users.FirstOrDefault(user => user.Id == request.UserId);
            var chat = chats.FirstOrDefault(chat => chat.Users.All(user => user.Id == request.FriendId || user.Id == request.UserId));

            var newMessage = new MessageDal()
            {
                User = currentUser,
                Text = request.Message
            };

            chat.Messages.Add(newMessage);

            await _context.SaveChangesAsync(cancellationToken);

            var friendDb = _context.Users.FirstOrDefault(user => user.Id == request.FriendId);

            await request.Clients.Clients(friendDb.ConnectionId).SendAsync("getMessageClient", request.Message);
            await request.Clients.Caller.SendAsync("sendMessageClientStatus", new { Success = true});

            return new AddMessageCommandResult(true);
        }
        catch (Exception ex)
        {
            return new AddMessageCommandResult(false);
        }
    }
}