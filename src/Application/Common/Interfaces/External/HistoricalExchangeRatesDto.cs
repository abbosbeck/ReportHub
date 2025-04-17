using System.Text.Json.Serialization;

namespace Application.Common.Interfaces.External;

public class HistoricalExchangeRatesDto
{
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("day")]
    public int Day { get; set; }

    [JsonPropertyName("base_code")]
    public string BaseCode { get; set; }

    [JsonPropertyName("requested_amount")]
    public double RequestedAmount { get; set; }

    [JsonPropertyName("conversion_amounts")]
    public Dictionary<string, double> ConversionAmounts { get; set; }
}
