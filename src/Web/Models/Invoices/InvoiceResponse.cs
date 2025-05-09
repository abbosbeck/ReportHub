namespace Web.Models.Invoices;

public class InvoiceResponse
{
    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public string CustomerName { get; set; }

    public Guid CustomerId { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
}
