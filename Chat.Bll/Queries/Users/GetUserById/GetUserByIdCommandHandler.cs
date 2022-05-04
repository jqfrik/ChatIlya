using MediatR;

namespace Chat.Bll.Queries.Users.GetUserById;

public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand,GetUserByIdCommandResult>
{
    public async Task<GetUserByIdCommandResult> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        var user = request.Context.Users.FirstOrDefault(x => x.Id == request.Id);
        if (user != null)
        {
            return new()
            {
                User = user?.Convert()
            };
        }
        return new();
    }
}