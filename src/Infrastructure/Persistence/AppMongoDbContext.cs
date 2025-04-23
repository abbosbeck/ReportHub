using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Persistence;

public class AppMongoDbContext(IOptions<KeyVaultOptions> keyVaultOptions)
{
    public IMongoDatabase Database => CreateDatabase(keyVaultOptions);

    private static IMongoDatabase CreateDatabase(IOptions<KeyVaultOptions> keyVaultOptions)
    {
        var connectionString = GetConnectionString(keyVaultOptions);

        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);

        return mongoClient.GetDatabase("reporthub");
    }

    private static string GetConnectionString(IOptions<KeyVaultOptions> keyVaultOptions)
    {
        var keyVaultUrl = keyVaultOptions.Value.KeyVaultURL;
        var keyVaultClientId = keyVaultOptions.Value.ClientId;
        var keyVaultClientSecret = keyVaultOptions.Value.ClientSecret;
        var keyVaultDirectoryId = keyVaultOptions.Value.DirectoryID;

        var credential = new ClientSecretCredential(
            keyVaultDirectoryId,
            keyVaultClientId,
            keyVaultClientSecret);
        var client = new SecretClient(new Uri(keyVaultUrl), credential);

        return client.GetSecret("MongoDb").Value.Value;
    }
}
