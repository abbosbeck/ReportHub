using Application.Common.Attributes;
using Application.Common.Constants;

namespace Application.WeatherForecast.GetWeatherForecast;

[AllowedFor(UserRoles.Admin)]
public class GetWeatherForecastRequest : IRequest<IEnumerable<WeatherForecast>>
{
}

public class GetWeatherForecastRequestHandler
    : IRequestHandler<GetWeatherForecastRequest, IEnumerable<WeatherForecast>>
{
    private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

    public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        })
        .ToArray();

        return await Task.FromResult(result);
    }
}
