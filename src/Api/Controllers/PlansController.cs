using Application.Plans.AddPlanItem;
using Application.Plans.CreatePlan;
using Application.Plans.DeletePlan;
using Application.Plans.DeletePlanItem;
using Application.Plans.GetPlanById;
using Application.Plans.GetPlansList;
using Application.Plans.UpdatePlan;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
[ApiController]
public class PlansController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid clientId, [FromBody] CreatePlanRequest request)
    {
        var result = await Mediator.Send(new CreatePlanCommand(clientId, request));

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid clientId, [FromBody] UpdatePlanRequest request)
    {
        var result = await Mediator.Send(new UpdatePlanCommand(clientId, request));

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetPlanByIdQuery(id, clientId));

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetListAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new GetPlansListQuery(clientId));

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeletePlanCommand(id, clientId));

        return Ok(result);
    }

    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> AddItemAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid clientId,
        [FromBody] AddPlanItemRequest request)
    {
        var result = await Mediator.Send(new AddPlanItemCommand(id, request, clientId));

        return Ok(result);
    }

    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> DeleteItemAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid itemId,
        [FromRoute] Guid clientId,
        [FromBody] AddPlanItemRequest request)
    {
        var result = await Mediator.Send(new DeletePlanItemCommand(id, itemId, clientId));

        return Ok(result);
    }
}
