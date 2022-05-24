using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Users.CheckChatWithCurrentFriend;

public class CheckChatWithCurrentFriendCommand : IRequest<CheckChatWithCurrentFriendResult>
{
    public string CurrentUserId { get; }
    public string FriendUserId { get; }
    public ChatContext Context { get; }

    public CheckChatWithCurrentFriendCommand(string currentUserId, string friendUserId, ChatContext context)
    {
        CurrentUserId = currentUserId;
        FriendUserId = friendUserId;
        Context = context;
    }
}