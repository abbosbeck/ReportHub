using Application.Common.Constants;
using Application.WeatherForecast.GetWeatherForecast;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class WeatherForecastController(ISender mediator) : ApiControllerBase(mediator)
{
    [Authorize(Roles = UserRoles.Admin)]
    [HttpGet(Name = "WeatherForecast")]
    public async Task<IActionResult> GetWeatherForecastAsync()
    {
        var result = await Mediator.Send(new GetWeatherForecastRequest());

        return Ok(result);
    }
}
