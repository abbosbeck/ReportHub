using Application.Plans.CreatePlan;
using Application.Plans.GetPlanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
[ApiController]
public class PlansController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid clientId, [FromBody] CreatePlanRequest request)
    {
        var result = await Mediator.Send(new CreatePlanCommand(clientId, request));

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetPlanByIdQuery { ClientId = clientId, Id = id });

        return Ok(result);
    }
}
