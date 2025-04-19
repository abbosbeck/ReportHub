using Newtonsoft.Json;

namespace Application.Common.Interfaces.External.Countries;

public class CountryDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("currencies")]
    public List<CurrencyDto> Currencies { get; set; }

    [JsonProperty("alpha3Code")]
    public string ThreeLetterCode { get; set; }
}