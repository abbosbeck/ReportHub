using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Infrastructure.Persistence.Configurations;

public static class LogMappingConfiguration
{
    public static void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Log)))
        {
            BsonClassMap.RegisterClassMap<Log>(cfg =>
            {
                cfg.MapIdProperty(log => log.Id)
                .SetElementName("_id")
                .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cfg.MapProperty(log => log.UserId)
                .SetElementName("user_id")
                .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cfg.MapProperty(log => log.InvoiceId)
                .SetElementName("invoice_id")
                .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cfg.MapProperty(log => log.TimeStamp)
                .SetElementName("time_stamp")
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local, BsonType.String));

                cfg.MapProperty(log => log.Status)
                .SetElementName("status");

                cfg.MapProperty(log => log.ClientId)
                .SetElementName("client_id")
                .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }
    }
}
