using Domain.Enums;

namespace Domain.Entities;

public class Log
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid InvoiceId { get; set; }

    public DateTime TimeStamp { get; set; }

    public LogStatus Status { get; set; }

    public Guid ClientId { get; set; }
}