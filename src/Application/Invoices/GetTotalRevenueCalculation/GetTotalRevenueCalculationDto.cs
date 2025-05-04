namespace Application.Invoices.GetTotalRevenueCalculation;

public class GetTotalRevenueCalculationDto(
    string totalRevenue,
    string currencyCode,
    DateTime startDate,
    DateTime endDate)
{
    public string TotalRevenue { get; set; } = totalRevenue;

    public string CurrencyCode { get; set; } = currencyCode;

    public DateTime StartDate { get; set; } = startDate;

    public DateTime EndDate { get; set; } = endDate;
}