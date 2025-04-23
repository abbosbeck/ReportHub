using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Enums;

public enum LogStatus
{
    /// <summary>
    /// Success of Export
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    Success,

    /// <summary>
    /// Failure of Export
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    Failure,
}