using Application.Clients.AddClientMember;
using Application.Clients.AssignClientRole;
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

    [HttpPost("add-member")]
    public async Task<IActionResult> AddMemberAsync([FromBody] AddClientMemberCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRoleAsync([FromBody] AssignClientRoleCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }
}
