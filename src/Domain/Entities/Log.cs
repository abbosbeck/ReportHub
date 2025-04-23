using Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Log
{
    [BsonId]
    [BsonElement("_id")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("user_id")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; init; }

    [BsonElement("invoice_id")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid InvoiceId { get; init; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local, Representation = BsonType.String)]
    public DateTime TimeStamp { get; init; }

    public LogStatus Status { get; set; }

    [BsonElement("client_id")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ClientId { get; init; }
}