using System.Collections.Concurrent;
using System.Globalization;

namespace Web.Services.ExternalServices;

public class MoneyService : IMoneyService
{
    private static readonly ConcurrentDictionary<string, CultureInfo> _currencyCultureCache = new(StringComparer.OrdinalIgnoreCase)
    {
        ["UZS"] = new CultureInfo("uz-Latn-UZ"),
        ["USD"] = new CultureInfo("chr-US"),
        ["EUR"] = new CultureInfo("ast-ES"),
    };

    public string GetAmountWithSymbol(decimal amount, string currencyCode)
    {
        var culture = _currencyCultureCache.GetOrAdd(currencyCode, code =>
        {
            var cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .FirstOrDefault(c => new RegionInfo(c.Name).ISOCurrencySymbol.Equals(
                    code, StringComparison.OrdinalIgnoreCase));

            return cultureInfo ?? CultureInfo.InvariantCulture;
        });

        return amount.ToString("C", culture);
    }
}
