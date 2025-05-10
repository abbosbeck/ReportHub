namespace Web.Models.Items;

public class ItemResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Price { get; set; }

    public string CurrencyCode { get; set; }

    public string CustomerName { get; set; }

    public Guid CustomerId { get; set; }
}
