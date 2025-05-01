using System.Globalization;
using System.Net.Http.Json;
using Application.Common.Exceptions;
using Application.Common.Interfaces.External.CurrencyExchange;

namespace Infrastructure.External;

public class CurrencyExchangeService(HttpClient httpClient) : ICurrencyExchangeService
{
    public async Task<bool> CheckCurrencyCodeAsync(string currencyCode)
    {
        var response = await httpClient.GetAsync($"latest/{currencyCode}");

        return response.IsSuccessStatusCode;
    }

    public async Task<HistoricalExchangeRatesDto> GetHistoricalExchangeRatesAsync(string currency, DateTime time, decimal amount)
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
        var currency = await GetHistoricalExchangeRatesAsync(source, time, amount);

        ValidateCurrencyCode(currency, source, destination);

        var exchangedCurrency = currency.ConversionAmounts[destination];

        return (decimal)exchangedCurrency;
    }

    public string GetAmountWithSymbol(decimal amount, string currencyCode)
    {
        var culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .FirstOrDefault(x => new RegionInfo(x.Name).ISOCurrencySymbol.Equals(
                currencyCode, StringComparison.OrdinalIgnoreCase));

        if (culture.Name == "uz-Cyrl-UZ")
        {
            culture = new CultureInfo("uz-Latn-UZ");
        }

        return amount.ToString("C", culture);
    }

    private static void ValidateCurrencyCode(HistoricalExchangeRatesDto currency, string source, string destination)
    {
        if (!currency.ConversionAmounts.ContainsKey(source))
        {
            throw new BadRequestException($"{source} is not supported by the given API");
        }

        if (!currency.ConversionAmounts.ContainsKey(destination))
        {
            throw new BadRequestException($"{destination} is not supported by the given API");
        }
    }
}
