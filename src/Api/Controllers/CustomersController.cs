using Application.Customers.AddCustomer;
using Application.Customers.GetCustomerList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CustomersController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] AddCustomerCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerList()
    {
        var result = await Mediator.Send(new GetCustomerListQuery());

        return Ok(result);
    }
}
