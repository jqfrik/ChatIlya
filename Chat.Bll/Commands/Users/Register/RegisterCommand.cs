using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Users.Register;

public class RegisterCommand : IRequest<RegisterCommandResult>
{
    
    public string Name { get; set; }
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public ChatContext Context { get; set; } 
    public RegisterCommand(string name, string login, string password, ChatContext context)
    {
        Name = name;
        Login = login;
        Password = password;
        Context = context;
    }
}