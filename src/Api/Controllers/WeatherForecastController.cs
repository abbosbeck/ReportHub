using Application.WeatherForecast.GetWeatherForecast;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class WeatherForecastController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet(Name = "WeatherForecast")]
    public async Task<IActionResult> GetWeatherForecastAsync()
    {
        var result = await Mediator.Send(new GetWeatherForecastRequest());

        return Ok(result);
    }
}
