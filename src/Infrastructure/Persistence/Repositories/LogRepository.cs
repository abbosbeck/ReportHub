using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class LogRepository(AppDbContext context) : ILogRepository
{
    public async Task<Log> AddAsync(Log log)
    {
        await context.AddAsync(log);
        await context.SaveChangesAsync();

        return log;
    }

    public async Task<Log> GetByIdAsync(Guid id)
    {
        return await context.Logs.FindAsync(id);
    }

    public IQueryable<Log> GetAll()
    {
        return context.Logs;
    }
}