using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Users.GetFriends;

public class GetFriendsByIdCommand : IRequest<GetFriendsByIdCommandResult>
{
    public Guid UserId;
    public ChatContext Context;

    public GetFriendsByIdCommand(Guid userId,ChatContext context)
    {
        UserId = userId;
        Context = context;
    }
}