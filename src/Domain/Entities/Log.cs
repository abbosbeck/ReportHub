using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Log
{
    public Guid UserId { get; init; }

    public Guid InvoiceId { get; init; }

    public DateTime TimeStamp { get; init; }

    public LogStatus Status { get; set; }

    public Guid ClientId { get; init; }
}