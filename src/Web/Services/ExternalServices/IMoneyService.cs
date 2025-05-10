namespace Web.Services.ExternalServices;

public interface IMoneyService
{
    string GetAmountWithSymbol(decimal amount, string currencyCode);
}