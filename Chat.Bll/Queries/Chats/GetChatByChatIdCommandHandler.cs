using Chat.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Chats;

public record GetChatByChatIdCommand(string ChatId, string? UserId, ChatContext Context) : IRequest<GetChatByChatIdResult>;

public record GetChatByChatIdResult(Domains.Chat? Chat);

internal sealed class GetChatByChatIdCommandHandler : IRequestHandler<GetChatByChatIdCommand,GetChatByChatIdResult>
{
    public Task<GetChatByChatIdResult> Handle(GetChatByChatIdCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ChatId))
        {
            return Task.FromResult(new GetChatByChatIdResult(null));
        }
        var currentChat = request.Context.Chats
            .Include(chat => chat.Messages)
            .Include(chat => chat.Users)
            .AsNoTracking()
            .AsEnumerable()
            .FirstOrDefault(chat => chat.Id == new Guid(request.ChatId));

        //У каждого пользователя отображается в названии диалога имя противоложного собеседника
        var friendNameForChatTitle = currentChat.Users.FirstOrDefault(user => user.Id != new Guid(request.UserId));
        currentChat.Title = friendNameForChatTitle.Name;
        return Task.FromResult(new GetChatByChatIdResult(currentChat?.Convert()));
    }
}