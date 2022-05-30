using Chat.Bll.Domains;
using Chat.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Users;

public record GetFriendsByIdCommand(Guid UserId, ChatContext Context) : IRequest<GetFriendsByIdCommandResult>;

public record GetFriendsByIdCommandResult(IEnumerable<User> Users);
internal sealed class GetFriendsByIdCommandHandler : IRequestHandler<GetFriendsByIdCommand,GetFriendsByIdCommandResult>
{
    public Task<GetFriendsByIdCommandResult> Handle(GetFriendsByIdCommand request, CancellationToken cancellationToken)
    {
        var user = request.Context.Users.Include(user => user.Users)
            .FirstOrDefault(user => user.Id == request.UserId);

        if (user == null)
            return Task.FromResult(new GetFriendsByIdCommandResult(new List<User>()));

        var domainUsers = user.Users.Convert();

        return Task.FromResult(new GetFriendsByIdCommandResult(domainUsers));
    }
}