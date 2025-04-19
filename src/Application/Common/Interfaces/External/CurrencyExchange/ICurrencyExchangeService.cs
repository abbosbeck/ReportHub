namespace Application.Common.Interfaces.External.CurrencyExchange;

public interface ICurrencyExchangeService
{
    Task<HistoricalExchangeRatesDto> GetHistoricalExchangeRatesAsync(string currency, DateTime time, decimal amount);

    Task<decimal> ExchangeCurrencyAsync(string source, string destination, decimal amount, DateTime time);
}
