using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IClientRepository
{
    Task<Client> GetByIdAsync(Guid clientId);

    Task<Client> AddAsync(Client client);

    Task<Client> UpdateAsync(Client client);

    Task<bool> DeleteAsync(Client client);

    IQueryable<Client> GetAll();
}
