namespace Web.Models.Invoices;

public class InvoiceCreateRequest
{
    public DateTime IssueDate { get; set; } = DateTime.Today;

    public DateTime DueDate { get; set; } = DateTime.Today;

    public Guid CustomerId { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }

    public List<Item> Items { get; set; } = new List<Item>();
}

public class Item
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string CurrencyCode { get; set; }
}