using Application.Users.ConfirmUserEmail;
using Application.Users.GetUserByEmail;
using Application.Users.GetUserByName;
using Application.Users.GiveRoleToUser;
using Application.Users.LoginUser;
using Application.Users.RefreshToken;
using Application.Users.RegisterUser;
using Application.Users.SoftDeleteUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetUserByName(string name)
    {
        var result = await Mediator.Send(new GetUserByNameQuery() { FirstName = name });

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var result = await Mediator.Send(new GetUserByEmailQuery() { Email = email });

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand request)
    {
        var result = await Mediator.Send(request);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand request)
    {
        var result = await Mediator.Send(request);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand refreshToken)
    {
        var result = await Mediator.Send(refreshToken);

        if (result == null)
        {
            return BadRequest("Refresh token wrong");
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] Guid id, [FromQuery] string token)
    {
        var result = await Mediator.Send(new ConfirmUserEmailQuery() { Token = token, UserId = id });

        return Ok(result);
    }

    [HttpPost("give-role")]
    public async Task<IActionResult> GiveRoleToUser([FromBody] GiveRoleToUserCommand giveRoleToUser)
    {
        var result = await Mediator.Send(giveRoleToUser);

        return Ok(result);
    }

    [HttpDelete("soft-delete-user")]
    public async Task<IActionResult> SoftDeleteUser([FromBody] SoftDeleteUserCommand softDeleteUserCommand)
    {
        var result = await Mediator.Send(softDeleteUserCommand);

        return Ok(result);
    }
}