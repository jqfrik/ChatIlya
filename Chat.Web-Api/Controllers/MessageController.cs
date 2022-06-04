using Chat.Bll.MailService;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private IMailService _mailService { get; }
    private IConfiguration Configuration { get; }

    public MessageController(IMailService service, IConfiguration configuration)
    {
        _mailService = service;
        Configuration = configuration;
    }

    [HttpGet("SendMessage")]
    public async Task SendMessage()
    {
        await _mailService.SendMail("pslava200000@yandex.ru", Configuration["EmailSettings:login"],
            Configuration["EmailSettings:password"], Configuration["EmailSettings:securePasswordYandex"], "СЛАВЯНДР",
            new[] { "pslava2000@mail.ru" }, "Заголовок", "Текстовое сообщение",
            false);
    }
}