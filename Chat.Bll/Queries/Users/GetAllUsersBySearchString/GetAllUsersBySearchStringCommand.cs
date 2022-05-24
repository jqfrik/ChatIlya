using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Users.GetAllUsersBySearchString;

public class GetAllUsersBySearchStringCommand : IRequest<GetAllUsersBySearchStringCommandResult>
{
    public string SearchString { get; }
    public string CurrentUserId { get; }
    public ChatContext Context { get; }

    public GetAllUsersBySearchStringCommand(string searchString, string currentUserId, ChatContext context)
    {
        SearchString = searchString;
        CurrentUserId = currentUserId;
        Context = context;
    }
}