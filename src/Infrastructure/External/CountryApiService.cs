using System.Net.Http.Json;
using Application.Common.Interfaces.External;

namespace Infrastructure.External;

public class CountryApiService(HttpClient httpClient) : ICountryApiService
{
    public async Task<CountryDto> GetByCode(string countryCode)
    {
        var response = await httpClient.GetAsync($"alpha/{countryCode}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CountryDto>();
    }
}