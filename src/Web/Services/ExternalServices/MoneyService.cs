using System.Globalization;

namespace Web.Services.ExternalServices;

public class MoneyService : IMoneyService
{
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
}
