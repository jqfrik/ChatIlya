using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Messages;

public record RemoveMessageCommand(string UserContextId, string ChatId, string MessageId) : IRequest<RemoveMessageCommandResult>;

public record RemoveMessageCommandResult(bool Success);

public class RemoveMessageCommandHandler : IRequestHandler<RemoveMessageCommand,RemoveMessageCommandResult>
{
    public ChatContext DbContext { get; }

    public RemoveMessageCommandHandler(ChatContext context)
    {
        DbContext = context;
    }
    public async Task<RemoveMessageCommandResult> Handle(RemoveMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var friendDb = DbContext.Users.FirstOrDefault(user => user.Id == new Guid(request.UserContextId));
            var chat = friendDb.Chats.FirstOrDefault(chat => chat.Id == new Guid(request.ChatId));
            var message = chat.Messages.FirstOrDefault(message => message.Id == new Guid(request.MessageId));
            chat.Messages.Remove(message);
            await DbContext.SaveChangesAsync(CancellationToken.None);
            return new RemoveMessageCommandResult(true);
        }
        catch
        {
            return new RemoveMessageCommandResult(false);
        }
    }
}