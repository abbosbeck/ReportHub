using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using MongoDB.Driver;
using Org.BouncyCastle.Utilities;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class LogRepository(AppMongoDbContext context) : ILogRepository
{
    private readonly IMongoCollection<Log> logs = context
        .Database?.GetCollection<Log>("log");

    public async Task<Log> AddAsync(Log log)
    {
        await logs.InsertOneAsync(log);

        return log;
    }

    public Log GetById(string id)
    {
        var filter = Builders<Log>.Filter.Eq(l => l.Id, id);
        var log = logs.Find(filter).FirstOrDefault();

        return log;
    }

    public async Task<IEnumerable<Log>> GetAllAsync()
    {
        return await logs.Find(FilterDefinition<Log>.Empty).ToListAsync();
    }
}