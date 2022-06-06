using System.Text;
using Chat.Bll.MailService;
using Chat.Dal;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Chat.Bll.Commands.Users;

public record ResetPasswordSecondStageCommand(string Email,string SmsChecker) : IRequest<ResetPasswordSecondStageCommandResult>;
public record ResetPasswordSecondStageCommandResult(bool Success);

internal sealed class ResetPasswordSecondStageCommandHandler : IRequestHandler<ResetPasswordSecondStageCommand,ResetPasswordSecondStageCommandResult>
{
    private ChatContext _context { get; }
    private IMailService _mailService { get; }
    public IConfiguration _configuration { get; }

    public ResetPasswordSecondStageCommandHandler(ChatContext context,IMailService mailService,IConfiguration configuration)
    {
        _context = context;
        _mailService = mailService;
        _configuration = configuration;
    }
    public async Task<ResetPasswordSecondStageCommandResult> Handle(ResetPasswordSecondStageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.SmsChecker))
        {
            return new ResetPasswordSecondStageCommandResult(false);
        }
        var user = _context.Users.FirstOrDefault(user => user.Email == request.Email);
        if (user != null && user.SmsChecker == request.SmsChecker)
        {
            var title = "Авторизационные данные";
            var textBuilder = new StringBuilder();
            var login = Guid.NewGuid();
            var password = Authorization.GenerateNewPassword(10);
            var hashPassword = Authorization.Encode(password);
            textBuilder.Append(string.Format("Новый Логин: {0}\n",login));
            textBuilder.Append(string.Format("Новый Пароль: {0}",password));
            user.SmsChecker = "";
            user.HashPassword = hashPassword;
            user.Login = login.ToString();
            await _context.SaveChangesAsync(CancellationToken.None);
            await _mailService.SendMail(_configuration["EmailSettings:currentActiveEmail"], _configuration["EmailSettings:login"],
                _configuration["EmailSettings:password"], _configuration["EmailSettings:securePasswordYandex"],
                _configuration["AppName"],
                new[] { request.Email }, title, textBuilder.ToString(),
                false);
            return (new ResetPasswordSecondStageCommandResult(true));
        }

        return new ResetPasswordSecondStageCommandResult(false);
    }
}