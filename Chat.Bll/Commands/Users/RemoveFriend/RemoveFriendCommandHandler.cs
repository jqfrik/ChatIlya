using MediatR;

namespace Chat.Bll.Commands.Users.RemoveFriend;

public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand,RemoveFriendCommandResult>
{
    public Task<RemoveFriendCommandResult> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        
    }
}