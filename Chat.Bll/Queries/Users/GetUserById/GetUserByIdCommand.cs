using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Users.GetUserById;

public class GetUserByIdCommand : IRequest<GetUserByIdCommandResult>
{
    public Guid Id { get; set; }
    
    public ChatContext Context { get; set; }

    public GetUserByIdCommand(Guid id,ChatContext context)
    {
        Id = id;
        Context = context;
    }
}