using System.IdentityModel.Tokens.Jwt;
using Chat.Bll;
using Chat.Bll.Commands.Users;
using Chat.Bll.Domains;
using Chat.Bll.Queries.Users;
using Chat.Bll.Requests;
using Chat.Dal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Authorization = Chat.Bll.Authorization;

namespace Chat.Web_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private ChatContext _context;
    private IMediator _mediator;

    public UsersController(ChatContext context,IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }
    
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerCommandResult = await _mediator.Send(new RegisterCommand(request.FullName, request.Login, request.Password,request.Email,request.Telephone, _context));
        if (registerCommandResult.Success)
        {
            return Ok(new { data = true});
        }
        return BadRequest(new { data = "Пользователь с таким логином уже существует" });
    }

    [AllowAnonymous]
    [HttpPost("ResetPasswordFirstStage")]
    public async Task<IActionResult> ResetPasswordFirstStage(ResetPasswordFirstStageRequest request)
    {
        //Нужна почта и телефон при авторизации
        var resetPasswordCommandResult = await _mediator.Send(new ResetPasswordFirstStageCommand(request.Email));
        if (string.IsNullOrEmpty(resetPasswordCommandResult.newHashedPassword))
        {
            return BadRequest(new { data = "Не удалось сбросить пароль" });
        }
        return Ok(new { data = true });
    }

    [AllowAnonymous]
    [HttpPost("ResetPasswordSecondStage")]
    public async Task<IActionResult> ResetPasswordSecondStage(ResetPasswordSecondStageRequest request)
    {
        var resetPasswordSecondStageResult = await _mediator.Send(new ResetPasswordSecondStageCommand(request.Email, request.SmsChecker));
        if (resetPasswordSecondStageResult.Success)
        {
            return Ok(new { data = true });
        }

        return BadRequest(new { data = false });
    }

    [AllowAnonymous]
    [HttpPost("Authentication")]
    public IActionResult Authentication(AuthenticationRequest request)
    {
        var identity = Authorization.GetIdentity(request.Login, request.Password, _context);
        if (identity == null)
        {
            return BadRequest(new { data = "Такого пользователя не найдено" });
        }
 
        var now = DateTime.UtcNow;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
        var response = new
        {
            access_token = encodedJwt,
            login = identity.Name,
            id = identity.Claims.FirstOrDefault(x => x.Type == "GUID")?.Value
        };
 
        return Ok(response);
    }

    [Authorize]
    [HttpPost("GetUserById")]
    public async Task<IActionResult> GetUserById(GetUserByIdRequest request)
    {
        var getUserByIdCommandResult = await _mediator.Send(new GetUserByIdCommand(request.Id,_context));
        if (getUserByIdCommandResult.User != null)
        {
            return Ok(getUserByIdCommandResult.User);
        }
        return BadRequest(new { errorText = "Не найдено пользователя с таким логином" });
    }

    [Authorize]
    [HttpPost("GetFriendsById")]
    public async Task<IActionResult> GetFriendsById(GetFriendsByIdRequest request)
    {
        var getFriendsByIdResult = await _mediator.Send(new GetFriendsByIdCommand(request.Id, _context));

        return Ok(getFriendsByIdResult.Users.ToList());
    }

    [Authorize]
    [HttpPost("GetAllUsersBySearchString")]
    public async Task<List<User>> GetAllUsersBySearchString(GetAllUsersBySearchStringRequest request)
    {
        var getAllUsersBySearchStringResult =
            await _mediator.Send(
                new GetAllUsersBySearchStringCommand(request.SearchString, request.CurrentUserId, _context));
        return getAllUsersBySearchStringResult.Users;
    }

    [Authorize]
    [HttpPost("AddFriend")]
    public async Task<IActionResult> AddFriend(AddFriendRequest request)
    {
        var addFriendCommandResult = await _mediator.Send(new AddFriendCommand(request.CurrentUserId, request.FriendId, _context));

        if (addFriendCommandResult.Success)
            return Ok(new {success = true});
        return BadRequest(new { errorText = "Не удалось добавить пользователя" });
    }

    [Authorize]
    [HttpPost("RemoveFriend")]
    public async Task<IActionResult> RemoveFriend(RemoveFriendRequest request)
    {
        var removeFriendCommandResult =
            await _mediator.Send(new RemoveFriendCommand(request.CurrentUserId, request.FriendId, _context));
        
        if (removeFriendCommandResult.Success)
            return Ok(new {success = true});
        return BadRequest(new { errorText = "Не удалось удалить пользователя" });
    }

    [HttpPost("UploadPhoto")]
    public async Task<IActionResult> UploadPhoto(UploadPhotoRequest request)
    {
        var uploadFileResult = await _mediator.Send(new UploadFileCommand(request.UserId, request.Src));
        return Ok(new { data = uploadFileResult.Success });
    }

    [Authorize]
    [HttpPost("SetStatus")]
    public async Task<IActionResult> SetStatus(SetStatusRequest request)
    {
        var currentUserId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "GUID")?.Value;
        var setStatusResult = await _mediator.Send(new SetStatusCommand(currentUserId, request.Active));
        if (setStatusResult.Success)
        {
            return Ok(new { data = true});
        }

        return BadRequest(new { errorText = "Не удалось установить статус" });
    }
}