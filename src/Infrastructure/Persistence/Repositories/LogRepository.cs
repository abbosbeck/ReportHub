using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class LogRepository: ILogRepository
{
    public async Task<Log> AddAsync(Log log)
    {
        throw new NotImplementedException();
    }

    public async Task<Log> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Log> GetAll()
    {
        throw new NotImplementedException();
    }
}