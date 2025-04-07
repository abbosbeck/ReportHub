using Application.Users.GetUserByName;
using Application.Users.GetUserByPhoneNumber;
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
    [HttpGet("{name}")]
    public async Task<IActionResult> GetUserByName(string name)
    {
        var result = await Mediator.Send(new GetUserByNameRequest() { FirstName = name });

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("phone/{phone}")]
    public async Task<IActionResult> GetUserByPhoneNumber(string phone)
    {
        var result = await Mediator.Send(new GetUserByPhoneNumberQuery() { PhoneNumber = phone });

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
    public async Task<IActionResult> Login(LoginUserQuery request)
    {
        var result = await Mediator.Send(request);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenQuery refreshToken)
    {
        var result = await Mediator.Send(refreshToken);

        if (result == null)
        {
            return BadRequest("Refresh token wrong");
        }

        return Ok(result);
    }

    [HttpPost("give-role")]
    public async Task<IActionResult> GiveRoleToUser([FromBody] GiveRoleToUserQuery giveRoleToUser)
    {
        var result = await Mediator.Send(giveRoleToUser);

        return Ok(result);
    }

    [HttpPost("soft-delete-user")]
    public async Task<IActionResult> SoftDeleteUser([FromBody] SoftDeleteUserQuery softDeleteUserCommand)
    {
        var result = await Mediator.Send(softDeleteUserCommand);

        return Ok(result);
    }
}
