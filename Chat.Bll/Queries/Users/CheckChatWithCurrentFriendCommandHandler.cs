using Chat.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Users;

public record CheckChatWithCurrentFriendCommand
    (string CurrentUserId, string FriendUserId, ChatContext Context) : IRequest<CheckChatWithCurrentFriendResult>;

public record CheckChatWithCurrentFriendResult(bool Success);

internal sealed class CheckChatWithCurrentFriendCommandHandler : IRequestHandler<CheckChatWithCurrentFriendCommand,CheckChatWithCurrentFriendResult>
{
    public Task<CheckChatWithCurrentFriendResult> Handle(CheckChatWithCurrentFriendCommand request, CancellationToken cancellationToken)
    {
        var currentUser = request.Context.Users
            .Include(x => x.Users)
            .AsNoTracking()
            .FirstOrDefault(user => user.Id == new Guid(request.CurrentUserId));
        if (currentUser.Users.Any(user => user.Id == new Guid(request.FriendUserId)))
        {
            return Task.FromResult(new CheckChatWithCurrentFriendResult(true));
        }

        return Task.FromResult(new CheckChatWithCurrentFriendResult(false));
    }
}