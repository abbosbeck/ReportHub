using Application.Customers.AddCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CustomersController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("add-customer")]
    public async Task<IActionResult> AddCustomer([FromBody] AddCustomerCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }
}
