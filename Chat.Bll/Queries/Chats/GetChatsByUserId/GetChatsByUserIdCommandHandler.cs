using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Chats.GetChatsByUserId;

public class GetChatsByUserIdCommandHandler : IRequestHandler<GetChatsByUserIdCommand,GetChatsByUserIdCommandResult>
{
    public Task<GetChatsByUserIdCommandResult> Handle(GetChatsByUserIdCommand request, CancellationToken cancellationToken)
    {
        var chats = request.Context.Chats.Include(chat => chat.Users)
            .Where(chat => chat.Users.Any(user => user.Id == request.UserId)).Convert();

        return Task.FromResult(new GetChatsByUserIdCommandResult()
        {
            Chats = chats
        });
    }
}