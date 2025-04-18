using Application.Customers.CreateCustomer;
using Application.Customers.DeleteCustomer;
using Application.Customers.GetCustomerById;
using Application.Customers.GetCustomerList;
using Application.Customers.UpdateCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("clients/{clientId:guid}/[controller]")]
public class CustomersController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid clientId, [FromBody] CreateCustomerRequest request)
    {
        var result = await Mediator.Send(new CreateCustomerCommand(clientId, request));

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid clientId, [FromBody] UpdateCustomerRequest request)
    {
        var result = await Mediator.Send(new UpdateCustomerCommand(clientId, request));

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetCustomerByIdQuery { Id = id, ClientId = clientId });

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromRoute] Guid clientId)
    {
        var result = await Mediator.Send(new GetCustomerListQuery { ClientId = clientId });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteCustomerCommand(id, clientId));

        return Ok(result);
    }
}