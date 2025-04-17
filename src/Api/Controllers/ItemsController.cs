using Application.Items.CreateItem;
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
}
