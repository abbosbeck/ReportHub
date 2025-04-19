namespace Application.Invoices.GetInvoicesList;

public class ItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string CurrencyCode { get; init; }
}