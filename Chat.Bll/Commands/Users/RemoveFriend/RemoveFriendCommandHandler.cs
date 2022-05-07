using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Users.RemoveFriend;

public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand,RemoveFriendCommandResult>
{
    public async Task<RemoveFriendCommandResult> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users = request.Context.Users.Include(user => user.Users);
            var user = users.FirstOrDefault(user => user.Id == request.UserId);
            var userForRemove = users.FirstOrDefault(user => user.Id == request.UserIdForRemove);
            if(user == null || userForRemove == null)
                return new RemoveFriendCommandResult()
                {
                    Success = true
                };
            user.Users.Remove(userForRemove);

            await request.Context.SaveChangesAsync();
            
            return new RemoveFriendCommandResult()
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new RemoveFriendCommandResult()
            {
                Success = false
            };
        }
    }
}