using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ISystemRoleRepository
{
    Task<SystemRole> GetByNameAsync(string name);
}
