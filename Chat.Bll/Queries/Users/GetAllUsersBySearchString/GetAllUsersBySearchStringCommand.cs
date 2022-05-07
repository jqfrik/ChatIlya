using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Users.GetAllUsersBySearchString;

public class GetAllUsersBySearchStringCommand : IRequest<GetAllUsersBySearchStringCommandResult>
{
    public string SearchString { get; }
    public Guid CurrentUserId { get; }
    public ChatContext Context { get; }

    public GetAllUsersBySearchStringCommand(string searchString, Guid currentUserId, ChatContext context)
    {
        SearchString = searchString;
        CurrentUserId = currentUserId;
        Context = context;
    }
}