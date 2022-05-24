using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Users.CheckChatWithCurrentFriend;

public class CheckChatWithCurrentFriendCommandHandler : IRequestHandler<CheckChatWithCurrentFriendCommand,CheckChatWithCurrentFriendResult>
{
    public Task<CheckChatWithCurrentFriendResult> Handle(CheckChatWithCurrentFriendCommand request, CancellationToken cancellationToken)
    {
        var currentUser = request.Context.Users
            .Include(x => x.Users)
            .AsNoTracking()
            .FirstOrDefault(user => user.Id == new Guid(request.CurrentUserId));
        if (currentUser.Users.Any(user => user.Id == new Guid(request.FriendUserId)))
        {
            return Task.FromResult(new CheckChatWithCurrentFriendResult()
            {
                Success = true
            });
        }

        return Task.FromResult(new CheckChatWithCurrentFriendResult()
        {
            Success = false
        });
    }
}