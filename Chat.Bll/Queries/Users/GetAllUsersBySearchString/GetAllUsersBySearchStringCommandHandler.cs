using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Users.GetAllUsersBySearchString;

public class GetAllUsersBySearchStringCommandHandler : IRequestHandler<GetAllUsersBySearchStringCommand,GetAllUsersBySearchStringCommandResult>
{
    public Task<GetAllUsersBySearchStringCommandResult> Handle(GetAllUsersBySearchStringCommand request, CancellationToken cancellationToken)
    {
        var matchedUsers = request.Context.Users
            .AsNoTracking()
            .Include(x => x.Chats)
            .Include(x => x.Users)
            .AsEnumerable()
            .Select(user => user.Convert())
            .Where(user => user.Name != null && user.Name.Contains(request.SearchString,StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        return Task.FromResult(new GetAllUsersBySearchStringCommandResult()
        {
            Users = matchedUsers
        });
    }
}