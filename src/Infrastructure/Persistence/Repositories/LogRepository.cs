using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using MongoDB.Driver;

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

    public async Task<Log> GetByIdAsync(Guid id, Guid clientId)
    {
        var filter = await logs.FindAsync(l => l.Id == id && l.ClientId == clientId);

        return await filter.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Log>> GetAllAsync(Guid clientId)
    {
        return await logs.Find(Builders<Log>.Filter.Eq(l => l.ClientId, clientId)).ToListAsync();
    }
}