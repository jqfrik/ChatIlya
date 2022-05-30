using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Users;

public record AddFriendCommand
    (Guid CurrentUserId, Guid FriendId, ChatContext Context) : IRequest<AddFriendCommandResult>;

public record AddFriendCommandResult(bool Success);

internal sealed class AddFriendCommandHandler : IRequestHandler<AddFriendCommand,AddFriendCommandResult>
{
    public async Task<AddFriendCommandResult> Handle(AddFriendCommand request, CancellationToken cancellationToken)
    {
        var currentUser = request.Context.Users.FirstOrDefault(x => x.Id == request.CurrentUserId);
        var friend = request.Context.Users.FirstOrDefault(x => x.Id == request.FriendId);

        if (currentUser == null || friend == null)
            return new(false);

        currentUser.Users.Add(friend);
        await request.Context.SaveChangesAsync(CancellationToken.None);
        return new AddFriendCommandResult(true);
    }
}