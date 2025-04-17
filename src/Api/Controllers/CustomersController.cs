using Application.Customers.CreateCustomer;
using Application.Customers.GetCustomerById;
using Application.Customers.UpdateCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CustomersController(ISender mediator) : ApiControllerBase(mediator)
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetCustomerByIdQuery { Id = id });

        return Ok(result);
    }
}