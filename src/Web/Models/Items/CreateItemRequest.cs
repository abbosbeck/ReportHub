namespace Web.Models.Items;

public class CreateItemRequest
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string CurrencyCode { get; set; }

    public Guid InvoiceId { get; set; }
}