using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ILogRepository
{
    Task<Log> AddAsync(Log log);

    Log GetById(Guid id, Guid clientId);

    Task<IEnumerable<Log>> GetAllAsync(Guid clientId);
}