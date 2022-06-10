using Chat.Bll.Domains;
using Chat.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Chats;

public record GetMessagesByChatIdCommand(string ChatId) : IRequest<GetMessagesByChatIdCommandResult>;

public record GetMessagesByChatIdCommandResult(IEnumerable<Message> Messages);

internal sealed class
    GetMessagesByChatId : IRequestHandler<GetMessagesByChatIdCommand, GetMessagesByChatIdCommandResult>
{
    private ChatContext _context { get; }

    public GetMessagesByChatId(ChatContext context)
    {
        _context = context;
    }

    public Task<GetMessagesByChatIdCommandResult> Handle(GetMessagesByChatIdCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ChatId))
        {
            return Task.FromResult(new GetMessagesByChatIdCommandResult(null));
        }

        var chatIdGuid = new Guid(request.ChatId);
        var chats = _context.Chats
            .Include(chat => chat.Users)
            .Include(chat => chat.Messages);
        var chat = chats.FirstOrDefault(chat => chat.Id == chatIdGuid);
        var messages = chat.Messages;
        var convertMessages = messages.Convert();
        var encodedMessages = convertMessages.Select(message =>
        {
            message.Text = Authorization.Encode(message.Text);
            return message;
        });

        return Task.FromResult(new GetMessagesByChatIdCommandResult(encodedMessages));
    }
}