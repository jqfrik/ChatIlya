using Chat.Dal;
using Chat.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Chats;

public record CreateChatWithFriendCommand
    (Guid MainUser, Guid SecondaryUser, ChatContext Context) : IRequest<CreateChatWithFriendCommandResult>;

public record CreateChatWithFriendCommandResult(Guid ChatId);

public class CreateChatWithFriendCommandHandler : IRequestHandler<CreateChatWithFriendCommand,CreateChatWithFriendCommandResult>
{
    public async Task<CreateChatWithFriendCommandResult> Handle(CreateChatWithFriendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users = request.Context.Users.Include(x => x.Users);
            var user = users.FirstOrDefault(user => user.Id == request.MainUser);
            var secondaryUser = users.FirstOrDefault(user => user.Id == request.SecondaryUser);

            if (user == null || secondaryUser == null)
                return new CreateChatWithFriendCommandResult(Guid.Empty);
            var chat = new ChatDal()
            {
                Id = Guid.NewGuid(),
                Users = new List<UserDal>()
                {
                    user,
                    secondaryUser
                },
            };
            await request.Context.Chats.AddAsync(chat,CancellationToken.None);

            await request.Context.SaveChangesAsync(CancellationToken.None);

            return new CreateChatWithFriendCommandResult(chat.Id.Value);
        }
        catch (Exception ex)
        {
            return new CreateChatWithFriendCommandResult(Guid.Empty);
        }
    }
}