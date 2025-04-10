using Application.Clients.AddClientMember;
using Application.Clients.GiveRoleToClientMember;
using Application.Clients.RegisterClient;
using Application.Clients.SoftDeleteClient;
using Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ClientsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("register-client")]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientCommand registerClientCommand)
    {
        var result = await Mediator.Send(registerClientCommand);
        
        return Ok(result);
    }

    [HttpPost("add-client-member")]
    public async Task<IActionResult> AddClientMember([FromBody] AddClientMemberCommand addClientMemberCommand)
    {
        var result = await Mediator.Send(addClientMemberCommand);

        return Ok(result);
    }

    [HttpPost("give-role")]
    public async Task<IActionResult> GiveRoleToClientMember([FromBody] GiveRoleToClientMemberCommand giveRoleToClientMemberCommand)
    {
        var result = await Mediator.Send(giveRoleToClientMemberCommand);

        return Ok(result);
    }

    [HttpDelete("soft-delete-client")]
    public async Task<IActionResult> SoftDeleteClient(Guid clientId)
    {
        var result = await Mediator.Send(new SoftDeleteClientCommand { ClientId = clientId });

        return Ok();
    }
}
