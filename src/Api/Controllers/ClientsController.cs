using Application.Clients.RegisterClient;
using Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ClientsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("create-client")]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientCommand registerClientCommand)
    {
        var result = await Mediator.Send(registerClientCommand);
        
        return Ok(result);
    }
}
