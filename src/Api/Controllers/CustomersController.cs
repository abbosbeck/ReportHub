using Application.Customers.CreateCustomer;
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
}