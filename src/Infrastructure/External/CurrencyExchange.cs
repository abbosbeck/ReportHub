using System.Net.Http.Json;
using Application.Common.Interfaces.External;

namespace Infrastructure.External;

public class CurrencyExchange(HttpClient httpClient) : ICurrencyExchange
{
    public async Task<HistoricalExchangeRatesDto> GetHistoricalExchangeRates(string currency, DateTime time, decimal amount)
    {
        var year = time.Year;
        var month = time.Month;
        var day = time.Day;

        var response = await httpClient.GetAsync($"history/{currency}/{year}/{month}/{day}/{amount}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<HistoricalExchangeRatesDto>();

        return result;
    }

    public async Task<decimal> ExchangeCurrencyAsync(string source, string destination, decimal amount, DateTime time)
    {
        var currency = await GetHistoricalExchangeRates(source, time, amount);
        var exchangedCurrency = currency.ConversionAmounts[destination];

        return (decimal)exchangedCurrency;
    }
}
