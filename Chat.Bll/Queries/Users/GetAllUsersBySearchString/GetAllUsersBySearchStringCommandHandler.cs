using MediatR;

namespace Chat.Bll.Queries.Users.GetAllUsersBySearchString;

public class GetAllUsersBySearchStringCommandHandler : IRequestHandler<GetAllUsersBySearchStringCommand,GetAllUsersBySearchStringCommandResult>
{
    public Task<GetAllUsersBySearchStringCommandResult> Handle(GetAllUsersBySearchStringCommand request, CancellationToken cancellationToken)
    {
        var matchedUsers =  request.Context.Users
            .Where(user => user.Name.Contains(request.SearchString,StringComparison.InvariantCultureIgnoreCase)).Convert();

        return Task.FromResult(new GetAllUsersBySearchStringCommandResult()
        {
            Users = matchedUsers
        });
    }
}