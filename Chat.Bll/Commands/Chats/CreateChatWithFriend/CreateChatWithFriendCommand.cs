using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Chats.CreateChatWithFriend;

public class CreateChatWithFriendCommand : IRequest<CreateChatWithFriendCommandResult>
{
    public Guid MainUser { get; }
    public Guid SecondaryUser { get; }
    public ChatContext Context { get; }

    public CreateChatWithFriendCommand(Guid mainUser, Guid secondaryUser,ChatContext context)
    {
        MainUser = mainUser;
        SecondaryUser = secondaryUser;
        Context = context;
    }
}