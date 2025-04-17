using Application.Clients.AddClientMember;
using Application.Clients.AssignClientRole;
using Application.Clients.CreateClient;
using Application.Clients.DeleteClient;
using Application.Clients.GetClientById;
using Application.Clients.UpdateClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
public class ClientsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody]CreateClientCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await Mediator.Send(new GetClientByIdQuery { ClientId = id });

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

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateClientCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await Mediator.Send(new DeleteClientCommand { ClientId = id });

        return Ok(result);
    }
}
