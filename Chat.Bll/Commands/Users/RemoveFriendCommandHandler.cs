using Chat.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Users;

public record RemoveFriendCommand
    (Guid UserId, Guid UserIdForRemove, ChatContext Context) : IRequest<RemoveFriendCommandResult>;

public record RemoveFriendCommandResult(bool Success);

internal sealed class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand,RemoveFriendCommandResult>
{
    public async Task<RemoveFriendCommandResult> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users = request.Context.Users.Include(user => user.Users);
            var user = users.FirstOrDefault(user => user.Id == request.UserId);
            var userForRemove = users.FirstOrDefault(user => user.Id == request.UserIdForRemove);
            if (user == null || userForRemove == null)
                return new RemoveFriendCommandResult(true);
            user.Users.Remove(userForRemove);

            await request.Context.SaveChangesAsync();

            return new RemoveFriendCommandResult(true);
        }
        catch (Exception ex)
        {
            return new RemoveFriendCommandResult(false);
        }
    }
}