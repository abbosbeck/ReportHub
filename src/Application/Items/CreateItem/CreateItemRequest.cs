namespace Application.Items.CreateItem;

public class CreateItemRequest
{
    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string CurrencyCode { get; init; }

    public Guid InvoiceId { get; init; }
}
