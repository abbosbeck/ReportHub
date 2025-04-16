using Application.Customers.CreateCustomer;
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
}