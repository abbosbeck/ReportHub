namespace Application.Common.Interfaces.External.Countries;

public interface ICountryService
{
    Task<CountryDto> GetByCodeAsync(string countryCode);
}