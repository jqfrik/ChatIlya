using Chat.Dal.Entities;
using MediatR;

namespace Chat.Bll.Commands.Users.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand,RegisterCommandResult>
{
    public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = request.Context.Users.FirstOrDefault(x => x.Login == request.Login);
        if (user != null)
            return new()
            {
                Success = false
            };

        var newUser = new UserDal()
        {
            Name = request.Name,
            Login = request.Login,
            HashPassword = Authorization.EncodePassword(request.Password)
        };
        await request.Context.Users.AddAsync(newUser,CancellationToken.None);
        await request.Context.SaveChangesAsync(CancellationToken.None);
        return new()
        {
            Success = true
        };
    }
}