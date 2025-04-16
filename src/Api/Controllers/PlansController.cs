using Application.Plans.CreatePlan;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PlansController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePlanCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }
}
