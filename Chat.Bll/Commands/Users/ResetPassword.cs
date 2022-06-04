using System.Text;
using Chat.Bll.MailService;
using Chat.Dal;
using Chat.Integrations.SmsAero;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Chat.Bll.Commands.Users;

public record ResetPasswordCommand(string userEmail) : IRequest<ResetPasswordCommandResult>;

public record ResetPasswordCommandResult(string newHashedPassword);

internal sealed class ResetPassword : IRequestHandler<ResetPasswordCommand,ResetPasswordCommandResult>
{
    private IMailService _mailService { get; }
    private IConfiguration _configuration { get; }
    public ChatContext _context { get; }
    public SmsAeroClient _aeroClient { get; }

    public ResetPassword(IMailService mailService, IConfiguration configuration, ChatContext context,SmsAeroClient aeroClient)
    {
        _mailService = mailService;
        _configuration = configuration;
        _context = context;
        _aeroClient = aeroClient;
    }
    public async Task<ResetPasswordCommandResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var title = "Авторизационные данные";
        var login = Guid.NewGuid();
        var password = Authorization.GenerateNewPassword(10);
        var decodedPassword = Authorization.EncodePassword(password);
        
        var textBuilder = new StringBuilder();
        textBuilder.Append(string.Format("Новый Логин: {0}\n",login));
        textBuilder.Append(string.Format("Новый Пароль: {0}",password));
        
        await _mailService.SendMail("pslava200000@yandex.ru", _configuration["EmailSettings:login"],
            _configuration["EmailSettings:password"], _configuration["EmailSettings:securePasswordYandex"],
            _configuration["AppName"],
            new[] { request.userEmail }, title, textBuilder.ToString(),
            false);
        await _aeroClient.SendMessage("+79514295341", decodedPassword);
        return new ResetPasswordCommandResult(decodedPassword);
    }
}