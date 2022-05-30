using Chat.Bll.Domains;
using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Users;

public record GetUserByIdCommand(Guid? Id ,ChatContext Context) : IRequest<GetUserByIdCommandResult>;

public record GetUserByIdCommandResult(User User);
internal sealed class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand,GetUserByIdCommandResult>
{
    public async Task<GetUserByIdCommandResult> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        var user = request.Context.Users.FirstOrDefault(x => x.Id == request.Id);
        if (user != null)
        {
            return new GetUserByIdCommandResult(user?.Convert());
        }
        return new GetUserByIdCommandResult(user?.Convert());
    }
}