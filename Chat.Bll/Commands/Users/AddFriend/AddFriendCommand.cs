using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Users.AddFriend;

public class AddFriendCommand : IRequest<AddFriendCommandResult>
{
    public Guid CurrentUserId { get; set; }
    
    public Guid FriendId { get; set; }
    
    public ChatContext Context { get; set; }
    public AddFriendCommand(Guid currentUserId, Guid friendId, ChatContext context)
    {
        CurrentUserId = currentUserId;
        FriendId = friendId;
        Context = context;
    }
}