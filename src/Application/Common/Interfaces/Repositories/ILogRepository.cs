using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ILogRepository
{
    Task<Log> AddAsync(Log log);

    Task<Log> GetByIdAsync(Guid id);

    IQueryable<Log> GetAll();
}