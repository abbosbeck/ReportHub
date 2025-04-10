using Application.Clients.LoginClient;
using Application.Clients.RegisterClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ClientsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientCommand registerClientCommand)
    {
        var result = await Mediator.Send(registerClientCommand);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginClientCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }
}
