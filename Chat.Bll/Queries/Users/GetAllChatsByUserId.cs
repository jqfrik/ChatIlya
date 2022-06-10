using Chat.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Queries.Users;

public record GetAllChatsByUserIdCommand(string UserId) : IRequest<GetAllChatsByUserIdCommandResult>;

public record GetAllChatsByUserIdCommandResult(IEnumerable<Domains.Chat> Chats);

internal sealed class
    GetAllChatsByUserIdCommandHandler : IRequestHandler<GetAllChatsByUserIdCommand, GetAllChatsByUserIdCommandResult>
{
    private ChatContext _dbContext { get; }

    public GetAllChatsByUserIdCommandHandler(ChatContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GetAllChatsByUserIdCommandResult> Handle(GetAllChatsByUserIdCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Empty;
        try
        {
            var userIdString = request.UserId.Replace("}", "").Replace("{", "");
            userId = new Guid(userIdString);
        }
        catch
        {
            return Task.FromResult(new GetAllChatsByUserIdCommandResult(new List<Domains.Chat>()));
        }

        var users = _dbContext.Users
            .Include(x => x.Chats)
            .Include(x => x.Users)
            .AsEnumerable()
            .FirstOrDefault(user => user.Id == userId);
        var changedUsers = users.Chats.AsEnumerable().Convert().Select(chat =>
        {
            chat.Title = chat.Users.FirstOrDefault(x => x.Id.Value != userId)?.Name;
            return chat;
        });
        return Task.FromResult(new GetAllChatsByUserIdCommandResult(changedUsers));
    }
}