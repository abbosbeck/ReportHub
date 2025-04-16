namespace Application.Common.Interfaces.External;

public interface ICountryApiService
{
    Task<CountryDto> GetByCode(string countryCode);
}