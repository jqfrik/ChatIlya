using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Users.GetFriends;

public class GetFriendsByIdCommandHandler : IRequestHandler<GetFriendsByIdCommand,GetFriendsByIdCommandResult>
{
    public Task<GetFriendsByIdCommandResult> Handle(GetFriendsByIdCommand request, CancellationToken cancellationToken)
    {
        var user = request.Context.Users.Include(user => user.Users)
            .FirstOrDefault(user => user.Id == request.UserId);

        if (user == null)
            return Task.FromResult(new GetFriendsByIdCommandResult());

        var domainUsers = user.Users.Convert();

        return Task.FromResult(new GetFriendsByIdCommandResult()
        {
            Users = domainUsers
        });
    }
}