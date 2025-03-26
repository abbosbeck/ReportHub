using Application.Users.GetUserByName;
using Application.WeatherForecast.GetWeatherForecast;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserByName(string name)
    {
        var result = await Mediator.Send(new GetUserByNameRequest(){FirstName = name});

        return Ok(result);
    }
}
