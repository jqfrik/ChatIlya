using Chat.Dal;
using Chat.Dal.Entities;
using MediatR;

namespace Chat.Bll.Commands.Users;

public record RegisterCommand
    (string FullName, string Login, string Password, string Email, string Telephone, ChatContext Context) : IRequest<RegisterCommandResult>;

public record RegisterCommandResult(bool Success);

public class RegisterCommandHandler : IRequestHandler<RegisterCommand,RegisterCommandResult>
{
    public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = request.Context.Users.FirstOrDefault(x => x.Login == request.Login);
        if (user != null)
            return new(false);

        var newUser = new UserDal()
        {
            Name = request.FullName,
            Login = request.Login,
            Email = request.Email,
            TelephoneNumber = request.Telephone,
            HashPassword = Authorization.Encode(request.Password)
        };
        await request.Context.Users.AddAsync(newUser,CancellationToken.None);
        await request.Context.SaveChangesAsync(CancellationToken.None);
        return new(true);
    }
}