using Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Log
{
    public Guid Id { get; set; }

    public Guid UserId { get; init; }

    public Guid InvoiceId { get; init; }

    public DateTime TimeStamp { get; init; }

    public LogStatus Status { get; set; }

    public Guid ClientId { get; init; }
}