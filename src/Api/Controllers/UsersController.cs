using Application.Users.AssignSystemRole;
using Application.Users.ConfirmUserEmail;
using Application.Users.DeleteUser;
using Application.Users.GetUserByEmail;
using Application.Users.LoginUser;
using Application.Users.RefreshToken;
using Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("{email}")]
    public async Task<IActionResult> GetByEmailAsync(string email)
    {
        var result = await Mediator.Send(new GetUserByEmailQuery() { Email = email });

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserCommand request)
    {
        var result = await Mediator.Send(request);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserCommand request)
    {
        var result = await Mediator.Send(request);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenCommand refreshToken)
    {
        var result = await Mediator.Send(refreshToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string token)
    {
        var result = await Mediator.Send(new ConfirmUserEmailQuery() { Token = token });

        return Ok(result);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRoleAsync([FromBody] AssignSystemRoleCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await Mediator.Send(new DeleteUserCommand { UserId = id });

        return Ok(result);
    }
}