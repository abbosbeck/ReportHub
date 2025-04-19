namespace Application.Items.GetItemsList;

public class ItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string CurrencyCode { get; init; }

    public Guid ClientId { get; init; }

    public Guid InvoiceId { get; init; }
}