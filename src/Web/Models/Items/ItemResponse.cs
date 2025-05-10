namespace Web.Models.Items;

public class ItemResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string PriceDto { get; set; }

    public string CurrencyCode { get; set; }

    public string InvoiceNumber { get; set; }

    public Guid InvoiceId { get; set; }
}
