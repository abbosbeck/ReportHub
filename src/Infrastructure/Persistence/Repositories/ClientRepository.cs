using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ClientRepository(AppDbContext context) : IClientRepository
{
    public async Task<Client> AddClientAdminAsync(Client client, Guid userId)
    {
        client.Id = Guid.NewGuid();

        await context.Clients.AddAsync(client);
        await RegisterClientRoleAssignmentAsync(client.Id, userId);

        await context.SaveChangesAsync();

        return await GetClientByIdAsync(client.Id);
    }

    public async Task<Client> GetClientByEmailAsync(string email)
    {
        return await context.Clients.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Client> GetClientByIdAsync(Guid Id)
    {
        return await context.Clients.FirstOrDefaultAsync(c => c.Id == Id);
    }

    private async Task RegisterClientRoleAssignmentAsync(Guid clientId, Guid userId)
    {
        var clientRole = await context.ClientRoles
            .FirstOrDefaultAsync(r => r.Name == "ClientAdmin");

        var clientRoleAssignment = new ClientRoleAssignment
        {
            UserId = userId,
            ClientRoleId = clientRole.Id,
            ClientId = clientId,
        };
        await context.ClientRoleAssignment.AddAsync(clientRoleAssignment);
    }
}
