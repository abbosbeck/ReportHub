using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Log : BaseEntity
{
    public Guid UserId { get; init; }

    public User User { get; init; }

    public Guid InvoiceId { get; init; }

    public Invoice Invoice { get; init; }

    public DateTime TimeStamp { get; init; }

    public LogStatus Status { get; set; }

    public Guid ClientId { get; init; }

    public Client Client { get; init; }
}