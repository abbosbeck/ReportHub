using Application.Users.GetUserByName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("{name}")]
    public async Task<IActionResult> GetUserByName(string name)
    {
        var result = await Mediator.Send(new GetUserByNameRequest() { FirstName = name });

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
