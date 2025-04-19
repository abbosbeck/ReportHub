using Application.Common.Interfaces.External;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;

namespace Application.WeatherForecast.GetWeatherForecast;

public class GetWeatherForecastRequest : IRequest<IEnumerable<WeatherForecast>>
{
}

public class GetWeatherForecastRequestHandler(
    ICurrencyExchangeService currencyExchangeService)
    : IRequestHandler<GetWeatherForecastRequest, IEnumerable<WeatherForecast>>
{
    private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

    public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var currency = await currencyExchangeService.ExchangeCurrencyAsync("USD", "UZS", 100, DateTime.Now);

        Console.WriteLine(currency);

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
