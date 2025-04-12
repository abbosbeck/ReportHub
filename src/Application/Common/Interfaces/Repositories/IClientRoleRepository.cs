using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IClientRoleRepository
{
    Task<ClientRole> GetByNameAsync(string name);
}
