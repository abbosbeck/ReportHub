using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Persistence;

public class AppMongoDbContext(IConfiguration configuration)
{
    public IMongoDatabase Database => CreateDatabase(configuration);

    public static IMongoDatabase CreateDatabase(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");

        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);

        return mongoClient.GetDatabase("reporthub");
    }
}
