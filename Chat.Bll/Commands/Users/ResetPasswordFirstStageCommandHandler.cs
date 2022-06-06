using System.Text;
using Chat.Bll.MailService;
using Chat.Dal;
using Chat.Integrations.SmsAero;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Chat.Bll.Commands.Users;

public record ResetPasswordFirstStageCommand(string userEmail) : IRequest<ResetPasswordFirstStageCommandResult>;

public record ResetPasswordFirstStageCommandResult(string newHashedPassword);

internal sealed class ResetPasswordFirstStageCommandHandler : IRequestHandler<ResetPasswordFirstStageCommand,ResetPasswordFirstStageCommandResult>
{
    private IMailService _mailService { get; }
    private IConfiguration _configuration { get; }
    public ChatContext _context { get; }
    public SmsAeroClient _aeroClient { get; }

    public ResetPasswordFirstStageCommandHandler(IMailService mailService, IConfiguration configuration, ChatContext context,SmsAeroClient aeroClient)
    {
        _mailService = mailService;
        _configuration = configuration;
        _context = context;
        _aeroClient = aeroClient;
    }
    public async Task<ResetPasswordFirstStageCommandResult> Handle(ResetPasswordFirstStageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.userEmail))
        {
            return new ResetPasswordFirstStageCommandResult(null);
        }
        var title = "Авторизационные данные";
        var login = Guid.NewGuid();
        var password = Authorization.GenerateNewPassword(10);
        var checkMessageTelephone = Authorization.GenerateSmsCheck(6);
        var decodedPassword = Authorization.Encode(password);
        
        var textBuilder = new StringBuilder();
        textBuilder.Append(string.Format("Новый Логин: {0}\n",login));
        textBuilder.Append(string.Format("Новый Пароль: {0}",password));
        
        var currentUser = _context.Users.FirstOrDefault(x => x.Email == request.userEmail);
        if (currentUser != null)
        {
            var userTelephone = currentUser.TelephoneNumber;

            var isValidSendSms = await _aeroClient.SendMessage(userTelephone, checkMessageTelephone);
            if (!isValidSendSms)
            {
                return new ResetPasswordFirstStageCommandResult(null);
            }

            currentUser.SmsChecker = checkMessageTelephone;
            await _context.SaveChangesAsync(CancellationToken.None);
        }
        return new ResetPasswordFirstStageCommandResult(checkMessageTelephone);
    }
}