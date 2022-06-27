using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Users;

public record SetStatusCommand(string UserId,bool Active) : IRequest<SetStatusCommandResult>;

public record SetStatusCommandResult(bool Success);

internal sealed class SetStatusCommandHandler : IRequestHandler<SetStatusCommand,SetStatusCommandResult>
{
    public ChatContext _dbContext { get; }

    public SetStatusCommandHandler(ChatContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<SetStatusCommandResult> Handle(SetStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Id == new Guid(request.UserId));
            if (user != null)
            {
                user.Active = request.Active;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return new(true);
        }
        catch
        {
            //ignore
        }

        return new(false);
    }
}