using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Users.RemoveFriend;

public class RemoveFriendCommand : IRequest<RemoveFriendCommandResult>
{
    public Guid UserId;
    public Guid UserIdForRemove;
    public ChatContext Context;

    public RemoveFriendCommand(Guid user,Guid userForRemove,ChatContext context)
    {
        UserId = user;
        UserIdForRemove = userForRemove;
        Context = context;
    }
}