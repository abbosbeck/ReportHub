using Application.Clients.AddClientMember;
using Application.Clients.GiveRoleToClientMember;
using Application.Clients.LoginClient;
using Application.Clients.RegisterClient;
using Application.Clients.SoftDeleteClient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ClientsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginClientCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("add-client-member")]
    public async Task<IActionResult> AddClientMember([FromBody] AddClientMemberCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("give-role")]
    public async Task<IActionResult> GiveRoleToClientMember([FromBody] GiveRoleToClientMemberCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("soft-delete")]
    public async Task<IActionResult> SoftDeleteClient(Guid clientId)
    {
        var result = await Mediator.Send(new SoftDeleteClientCommand { ClientId = clientId });

        return Ok(result);
    }
}
