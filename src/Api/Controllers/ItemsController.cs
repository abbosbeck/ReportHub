using Application.Items.CreateItem;
using Application.Items.DeleteItem;
using Application.Items.GetItemById;
using Application.Items.GetItemsList;
using Application.Items.UpdateItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
public class ItemsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid clientId, [FromBody] CreateItemRequest request)
    {
        var result = await Mediator.Send(new CreateItemCommand(clientId, request));

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid clientId, [FromBody] UpdateItemRequest request)
    {
        var result = await Mediator.Send(new UpdateItemCommand(clientId, request));

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetItemByIdQuery(id, clientId));

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new GetItemListQuery(clientId));

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteItemCommand(id, clientId));

        return Ok(result);
    }
}
