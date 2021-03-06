using Chat.Dal;
using Chat.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Chats;

public record CreateChatWithFriendCommand
    (Guid MainUser, Guid SecondaryUser) : IRequest<CreateChatWithFriendCommandResult>;

public record CreateChatWithFriendCommandResult(Guid ChatId);

public class CreateChatWithFriendCommandHandler : IRequestHandler<CreateChatWithFriendCommand,CreateChatWithFriendCommandResult>
{
    private ChatContext Context { get; }

    public CreateChatWithFriendCommandHandler(ChatContext context)
    {
        Context = context;
    }
    public async Task<CreateChatWithFriendCommandResult> Handle(CreateChatWithFriendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users = Context.Users.Include(x => x.Users);
            var chats = Context.Chats.Include(x => x.Users);
            var user = users.FirstOrDefault(user => user.Id == request.MainUser);
            var secondaryUser = users.FirstOrDefault(user => user.Id == request.SecondaryUser);

            var chatExists = chats.Any(chat =>
                chat.Users.Any(user => user.Id == request.SecondaryUser) &&
                chat.Users.Any(user => user.Id == request.MainUser));
            if (chatExists)
            {
                return new CreateChatWithFriendCommandResult(Guid.Empty);
            }
            if (user == null || secondaryUser == null)
                return new CreateChatWithFriendCommandResult(Guid.Empty);
            
            var chat = new ChatDal()
            {
                Title = secondaryUser.Name,
                Id = Guid.NewGuid(),
                Users = new List<UserDal>()
                {
                    user,
                    secondaryUser
                },
            };
            await Context.Chats.AddAsync(chat,CancellationToken.None);

            await Context.SaveChangesAsync(CancellationToken.None);

            return new CreateChatWithFriendCommandResult(chat.Id.Value);
        }
        catch (Exception ex)
        {
            return new CreateChatWithFriendCommandResult(Guid.Empty);
        }
    }
}