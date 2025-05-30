﻿namespace Web.Models.Invoices;

public class InvoiceResponse
{
    public Guid Id { get; set; }

    public int InvoiceNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public string AmountDto { get; set; }

    public string CurrencyCode { get; set; }

    public string CustomerName { get; set; }

    public Guid CustomerId { get; set; }

    public InvoicePaymentStatus PaymentStatus { get; set; }
}
