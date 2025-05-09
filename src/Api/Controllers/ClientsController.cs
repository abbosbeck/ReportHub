using Application.Clients.AddClientMember;
using Application.Clients.AssignClientRole;
using Application.Clients.CreateClient;
using Application.Clients.DeleteClient;
using Application.Clients.GetClientById;
using Application.Clients.GetClientByUserId;
using Application.Clients.GetClientsList;
using Application.Clients.UpdateClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
public class ClientsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateClientCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetClientByIdQuery(id));

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await Mediator.Send(new GetClientsListQuery());

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyClientsAsync()
    {
        var result = await Mediator.Send(new GetClientByUserIdQuery());

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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteClientCommand(id));

        return Ok(result);
    }
}
