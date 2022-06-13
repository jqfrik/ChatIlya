using Chat.Bll.Domains;
using Chat.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Users;

public record GetAllUsersBySearchStringCommand(string SearchString, string CurrentUserId, ChatContext Context):IRequest<GetAllUsersBySearchStringCommandResult>;

public record GetAllUsersBySearchStringCommandResult(List<User> Users);

internal sealed class GetAllUsersBySearchStringCommandHandler : IRequestHandler<GetAllUsersBySearchStringCommand,GetAllUsersBySearchStringCommandResult>
{
    public Task<GetAllUsersBySearchStringCommandResult> Handle(GetAllUsersBySearchStringCommand request, CancellationToken cancellationToken)
    {
        var matchedUsers = request.Context.Users
            .AsNoTracking()
            .AsEnumerable()
            .Select(user => user.Convert())
            .Where(user => user.Name != null && user.Id != new Guid(request.CurrentUserId) && user.Name.Contains(request.SearchString,StringComparison.InvariantCultureIgnoreCase))
            .ToList();
        return Task.FromResult(new GetAllUsersBySearchStringCommandResult(matchedUsers));
    }
}