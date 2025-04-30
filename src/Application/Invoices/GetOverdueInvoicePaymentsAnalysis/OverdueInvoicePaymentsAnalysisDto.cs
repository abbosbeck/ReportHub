namespace Application.Invoices.GetOverdueInvoicePaymentsAnalysis;

public class OverdueInvoicePaymentsAnalysisDto
{
    public int NumberOfInvoices { get; init; }

    public string CurrencyCode { get; init; }

    public decimal TotalAmount { get; init; }
}