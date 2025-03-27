using Application.Users.GetUserByName;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet("{name}")]
    public async Task<IActionResult> GetUserByName(string name)
    {
        var result = await Mediator.Send(new GetUserByNameRequest() { FirstName = name });

        return Ok(result);
    }
}
