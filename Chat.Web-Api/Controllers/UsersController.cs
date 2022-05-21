using System.IdentityModel.Tokens.Jwt;
using Chat.Bll;
using Chat.Bll.Commands.Users.AddFriend;
using Chat.Bll.Commands.Users.Register;
using Chat.Bll.Commands.Users.RemoveFriend;
using Chat.Bll.Dtos;
using Chat.Bll.Queries.Users.GetAllUsersBySearchString;
using Chat.Bll.Queries.Users.GetFriends;
using Chat.Bll.Queries.Users.GetUserById;
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
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerCommandResult = await _mediator.Send(new RegisterCommand(request.Name, request.Login, request.Password, _context));
        if (registerCommandResult.Success)
        {
            return Ok(new {success = true});
        }
        return BadRequest(new { errorText = "Пользователь с таким логином уже существует" });
    }

    [AllowAnonymous]
    [HttpGet("ResetPassword")]
    public IActionResult ResetPassword()
    {
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("Authentication")]
    public IActionResult Authentication(AuthenticationRequest request)
    {
        var identity = Authorization.GetIdentity(request.Login, request.Password, _context);
        if (identity == null)
        {
            return BadRequest(new { errorText = "Такого пользователя не найдено" });
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
        return BadRequest(new { errorText = "Пользователь с таким логином уже" });
    }

    [Authorize]
    [HttpPost("GetFriendsById")]
    public async Task<IActionResult> GetFriendsById(GetFriendsByIdRequest request)
    {
        var getFriendsByIdResult = await _mediator.Send(new GetFriendsByIdCommand(request.Id, _context));

        return Ok(getFriendsByIdResult.Users.ToList());
    }

    [HttpPost("GetAllUsersBySearchString")]
    public async Task<IActionResult> GetAllUsersBySearchString(GetAllUsersBySearchStringRequest request)
    {
        var getAllUsersBySearchStringResult =
            await _mediator.Send(
                new GetAllUsersBySearchStringCommand(request.SearchString, request.CurrentUserId, _context));
        return Ok(getAllUsersBySearchStringResult.Users);
    }

    [Authorize]
    [HttpPost("AddFriend")]
    public async Task<IActionResult> AddFriend(AddFriendRequest request)
    {
        var addFriendCommandResult = await _mediator.Send(new AddFriendCommand(request.CurrentUserId, request.FriendId, _context));

        if (addFriendCommandResult.Success)
            return Ok(new {success = true});
        return new StatusCodeResult(500);
    }

    [Authorize]
    [HttpPost("RemoveFriend")]
    public async Task<IActionResult> RemoveFriend(RemoveFriendRequest request)
    {
        var removeFriendCommandResult =
            await _mediator.Send(new RemoveFriendCommand(request.CurrentUserId, request.FriendId, _context));
        
        if (removeFriendCommandResult.Success)
            return Ok(new {success = true});
        return new StatusCodeResult(500);
    }
}