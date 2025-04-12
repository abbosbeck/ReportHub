using Application.Clients.CreateClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ClientsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody]CreateClientCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }
}
