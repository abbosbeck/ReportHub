using Application.Items.CreateItem;
using Application.Items.GetItemsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ItemsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateItemCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync(Guid clientId)
    {
        var result = await Mediator.Send(new GetItemListQuery { ClientId = clientId });

        return Ok(result);
    }
}
