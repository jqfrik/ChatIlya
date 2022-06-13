using Chat.Dal;
using Chat.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Messages;

public record AddMessageCommand(Guid? UserId, Guid? FriendId, string Message) : IRequest<AddMessageCommandResult>;

public record AddMessageCommandResult(bool Success);
public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand,AddMessageCommandResult>
{ 
    private ChatContext _context { get; }
    private IHubContext<ChatHub> _hubContext { get; }

    public AddMessageCommandHandler(ChatContext context,IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
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
            var chat = chats.FirstOrDefault(chat => chat.Users.Any(user => user.Id == request.FriendId) && chat.Users.Any(user => user.Id == request.UserId));

            var newMessage = new MessageDal()
            {
                User = currentUser,
                Text = request.Message,
                CreatedDate = DateTime.Now
            };

            chat.Messages.Add(newMessage);

            await _context.SaveChangesAsync(cancellationToken);

            var friendDb = _context.Users.FirstOrDefault(user => user.Id == request.FriendId);

            await _hubContext.Clients.Client(friendDb.ConnectionId).SendAsync("sendMessageClientStatus", new { success = true ,chatId = chat.Id });
            await _hubContext?.Clients.Client(currentUser.ConnectionId)
                .SendAsync("sendMessageClientStatus", new { success = true ,chatId = chat.Id });
            //await request?.Clients.Clients?.SendAsync("sendMessageClientStatus", new { Success = true});

            return new AddMessageCommandResult(true);
        }
        catch (Exception ex)
        {
            return new AddMessageCommandResult(false);
        }
    }
}