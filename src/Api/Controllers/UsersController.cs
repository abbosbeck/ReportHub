using Application.Users.GetUserByName;
using Application.Users.LoginUser;
using Application.Users.RefreshToken;
using Application.Users.RegisterUser;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

public class UsersController(ISender mediator) : ApiControllerBase(mediator)
{

    [Authorize]
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

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommandRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommandReqest request)
    {
        try
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshToken) 
    {
        var result = await Mediator.Send(refreshToken);

        if (result == null) 
        {
            return BadRequest("Refresh token wrong");
        }

        return Ok(result);
    }
}
