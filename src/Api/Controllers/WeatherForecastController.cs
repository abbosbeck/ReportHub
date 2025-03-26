using Application.WeatherForecast.GetWeatherForecast;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ApiControllerBase
{
    [HttpGet(Name = "WeatherForecast")]
    public async Task<IActionResult> GetWeatherForecastAsync()
    {
        var result = await Mediator.Send(new GetWeatherForecastRequest());

        return Ok(result);
    }
}
