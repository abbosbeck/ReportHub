namespace Application.Common.Interfaces.External.Countries;

public interface ICountryService
{
    Task<CountryDto> GetByCode(string countryCode);
}