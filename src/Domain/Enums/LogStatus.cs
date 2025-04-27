using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Enums;

public enum LogStatus
{
    /// <summary>
    /// Success of Export
    /// </summary>
    Success,

    /// <summary>
    /// Failure of Export
    /// </summary>
    Failure,
}