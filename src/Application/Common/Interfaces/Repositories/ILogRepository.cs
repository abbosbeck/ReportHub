using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ILogRepository
{
    Task<Log> AddAsync(Log log);

    Log GetById(string id);

    Task<IEnumerable<Log>> GetAllAsync();
}