using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Chats.GetChatsByUserId;

public class GetChatsByUserIdCommand : IRequest<GetChatsByUserIdCommandResult>
{
    public Guid UserId { get; }
    public ChatContext Context { get; }

    public GetChatsByUserIdCommand(Guid userId, ChatContext context)
    {
        UserId = userId;
        Context = context;
    }
}