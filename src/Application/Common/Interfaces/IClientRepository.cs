using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IClientRepository
{
    Task<Client> AddClientAdminAsync(Client client, Guid userId);

    Task AddClientMemberAsync(Client client);

    Task<Client> GetClientByEmailAsync(string email);

    Task<Client> GetClientByIdAsync(Guid id);

    Task<bool> GiveRoleToClientMemberAsync(Guid clientId, string roleName);

    Task<bool> SoftDeleteClientAsync(Guid id);

    Task UpdateClientAsync(Client client);
}
