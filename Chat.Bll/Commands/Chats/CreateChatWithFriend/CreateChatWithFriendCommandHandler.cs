using Chat.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Bll.Commands.Chats.CreateChatWithFriend;

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
                return new CreateChatWithFriendCommandResult()
                {
                    Success = false
                };
            var chat = new ChatDal()
            {
                Users = new List<UserDal>()
                {
                    user,
                    secondaryUser
                },
            };
            await request.Context.Chats.AddAsync(chat,CancellationToken.None);

            await request.Context.SaveChangesAsync(CancellationToken.None);

            return new CreateChatWithFriendCommandResult()
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new CreateChatWithFriendCommandResult()
            {
                Success = false
            };
        }
    }
}