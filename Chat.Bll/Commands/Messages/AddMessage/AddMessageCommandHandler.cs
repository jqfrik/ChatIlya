using Chat.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Messages.AddMessage;

public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand,AddMessageCommandResult>
{
    public async  Task<AddMessageCommandResult> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var chats = request.Context.Chats.Include(x => x.Users).Include(x => x.Messages);
            var chat = chats.FirstOrDefault(chat => chat.Users.All(user => user.Id == request.FriendId || user.Id == request.UserId));

            var newMessage = new MessageDal()
            {
                Text = request.Message
            };

            chat.Messages.Add(newMessage);

            await request.Context.SaveChangesAsync(cancellationToken);

            var friendDb = request.Context.Users.FirstOrDefault(user => user.Id == request.FriendId);

            await request.Clients.Clients(friendDb.ConnectionId).SendAsync("getMessageClient", request.Message);
            await request.Clients.Caller.SendAsync("sendMessageClientStatus", new { Success = true});

            return new AddMessageCommandResult()
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new AddMessageCommandResult()
            {
                Success = false
            };
        }
    }
}