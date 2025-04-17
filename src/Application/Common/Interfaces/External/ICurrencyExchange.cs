namespace Application.Common.Interfaces.External;

public interface ICurrencyExchange
{
    Task<HistoricalExchangeRatesDto> GetHistoricalExchangeRates(string currency, DateTime time, decimal amount);

    Task<decimal> ExchangeCurrency(string source, string destination, decimal amount, DateTime time);
}
