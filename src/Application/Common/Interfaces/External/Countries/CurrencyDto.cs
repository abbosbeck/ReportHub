using Newtonsoft.Json;

namespace Application.Common.Interfaces.External.Countries;

public class CurrencyDto
{
    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }
}