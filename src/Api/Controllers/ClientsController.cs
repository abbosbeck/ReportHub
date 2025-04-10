using Application.Clients.AddClientMember;
using Application.Clients.RegisterClient;
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
}
