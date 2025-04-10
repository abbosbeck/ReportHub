using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ClientRepository(AppDbContext context) : IClientRepository
{
    public async Task UpdateClientAsync(Client client)
    {
        context.Clients.Update(client);
        await context.SaveChangesAsync();
    }

    public async Task<Client> AddClientAdminAsync(Client client, Guid userId)
    {
        client.Id = Guid.NewGuid();

        await context.Clients.AddAsync(client);
        await RegisterClientRoleAssignmentAsync(client.Id, userId, "ClientAdmin");

        await context.SaveChangesAsync();

        return await GetClientByIdAsync(client.Id);
    }

    public async Task AddClientMemberAsync(Client client)
    {
        await context.AddAsync(client);
        await context.SaveChangesAsync();
    }

    public async Task<Client> GetClientByEmailAsync(string email)
    {
        return await context.Clients.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Client> GetClientByIdAsync(Guid id)
    {
        return await context.Clients.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> GiveRoleToClientMemberAsync(Guid clientId, string roleName)
    {
        Guid userId = clientId;
        await RegisterClientRoleAssignmentAsync(clientId, userId, roleName);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SoftDeleteClientAsync(Guid id)
    {
        var client = await GetClientByIdAsync(id);
        await UpdateClientAsync(client);
        await context.SaveChangesAsync();

        return true;
    }

    private async Task RegisterClientRoleAssignmentAsync(Guid clientId, Guid userId, string roleName)
    {
        var clientRole = await context.ClientRoles
            .FirstOrDefaultAsync(r => r.Name == roleName);
        if (clientRole is null)
        {
            throw new NotFoundException("There is no role with this name.");
        }

        var clientRoleAssignment = new ClientRoleAssignment
        {
            UserId = userId,
            ClientRoleId = clientRole.Id,
            ClientId = clientId,
        };
        await context.ClientRoleAssignment.AddAsync(clientRoleAssignment);
    }
}
