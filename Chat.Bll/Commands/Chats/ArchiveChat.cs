using System.Text;
using Chat.Dal;
using Chat.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Chats;

public record ArchiveChatCommand(string CurrentUserId, string ChatId) : IRequest<ArchiveChatCommandResult>;

public record ArchiveChatCommandResult(byte[] bytes);

internal sealed class ArchiveChat : IRequestHandler<ArchiveChatCommand,ArchiveChatCommandResult>
{
    public ChatContext Context { get; }

    public ArchiveChat(ChatContext context)
    {
        Context = context;
    }
    public Task<ArchiveChatCommandResult> Handle(ArchiveChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var chats = Context.Chats
                .Include(chat => chat.Users)
                .Include(chat => chat.Messages)
                .AsEnumerable();
            var guidChat = new Guid(request.ChatId);
            var chat = chats.FirstOrDefault(chat => chat.Id == guidChat);

            var resultText = new StringBuilder();
            var messages = chat.Messages;
            foreach (var messageItem in messages)
            {
                resultText.Append(messageItem?.User?.Name);
                resultText.Append("\n");
                resultText.Append(messageItem.Text);
                resultText.Append("----------\n");
            }
            var messagesInBytes = Encoding.UTF8.GetBytes(resultText.ToString());
            return Task.FromResult(new ArchiveChatCommandResult(messagesInBytes));
        }
        catch
        {
            return Task.FromResult(new ArchiveChatCommandResult(null));
        }
    }
}