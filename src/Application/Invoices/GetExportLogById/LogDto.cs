using Domain.Enums;

namespace Application.Invoices.GetExportLogById;

public class LogDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public Guid InvoiceId { get; init; }

    public DateTime TimeStamp { get; init; }

    public LogStatus LogStatus { get; set; }

    public Guid ClientId { get; init; }
}