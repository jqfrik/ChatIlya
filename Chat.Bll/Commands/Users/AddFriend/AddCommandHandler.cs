using MediatR;

namespace Chat.Bll.Commands.Users.AddFriend;

public class AddCommandHandler : IRequestHandler<AddFriendCommand,AddFriendCommandResult>
{
    public async Task<AddFriendCommandResult> Handle(AddFriendCommand request, CancellationToken cancellationToken)
    {
        var currentUser = request.Context.Users.FirstOrDefault(x => x.Id == request.CurrentUserId);
        var friend = request.Context.Users.FirstOrDefault(x => x.Id == request.FriendId);

        if (currentUser == null || friend == null)
            return new()
            {
                Success = false
            };
        
        currentUser.Users.Add(friend);
        await request.Context.SaveChangesAsync(CancellationToken.None);
        return new AddFriendCommandResult()
        {
            Success = true
        };
    }
}