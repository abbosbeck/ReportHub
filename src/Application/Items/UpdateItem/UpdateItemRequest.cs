namespace Application.Items.UpdateItem;

public class UpdateItemRequest
{
    public Guid Id { get; set; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public string CurrencyCode { get; init; }
}
