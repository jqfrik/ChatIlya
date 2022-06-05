using System.Security.Claims;
using Chat.Bll.Commands.Messages;
using Chat.Bll.MailService;
using Chat.Dal;
using Chat.Integrations.SmsAero.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private IMailService _mailService { get; }
    private IConfiguration _configuration { get; }
    private IMediator _mediator { get; }

    public MessageController(IMailService service, IConfiguration configuration, IMediator mediator)
    {
        _mailService = service;
        _configuration = configuration;
        mediator = mediator;
    }
}