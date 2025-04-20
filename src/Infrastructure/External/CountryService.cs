using System.Net.Http.Json;
using Application.Common.Interfaces.External.Countries;

namespace Infrastructure.External;

public class CountryService(HttpClient httpClient) : ICountryService
{
    public async Task<CountryDto> GetByCodeAsync(string countryCode)
    {
        var response = await httpClient.GetAsync($"alpha/{countryCode}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CountryDto>();
    }

    public async Task<string> GetCurrencyCodeByCountryCodeAsync(string code)
    {
        var customerCountry = await GetByCodeAsync(code);

        return customerCountry.Currencies
            .Select(c => c.Code)
            .FirstOrDefault();
    }
}