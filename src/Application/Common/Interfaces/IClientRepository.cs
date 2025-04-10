using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IClientRepository
{
    Task<Client> AddClientAdminAsync(Client client, Guid userId);

    Task<Client> GetClientByEmailAsync(string email);

    Task<Client> GetClientByIdAsync(Guid id);
}
