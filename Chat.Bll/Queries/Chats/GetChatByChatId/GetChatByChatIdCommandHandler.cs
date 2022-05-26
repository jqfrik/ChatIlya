using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Chats.GetChatByChatId;

public class GetChatByChatIdCommandHandler : IRequestHandler<GetChatByChatIdCommand,GetChatByChatIdResult>
{
    public Task<GetChatByChatIdResult> Handle(GetChatByChatIdCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ChatId))
        {
            return Task.FromResult(new GetChatByChatIdResult()
            {
                Chat = null
            });
        }
        var currentChat = request.Context.Chats
            .Include(chat => chat.Messages)
            .AsNoTracking()
            .AsEnumerable()
            .FirstOrDefault(chat => chat.Id == new Guid(request.ChatId));
        return Task.FromResult(new GetChatByChatIdResult()
        {
            Chat = currentChat?.Convert()
        });
    }
}