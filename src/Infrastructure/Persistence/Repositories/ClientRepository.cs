using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ClientRepository(AppDbContext context) : IClientRepository
{
    public async Task<Client> AddAsync(Client client)
    {
        await context.AddAsync(client);
        await context.SaveChangesAsync();

        return client;
    }

    public async Task<bool> DeleteAsync(Client client)
    {
        context.Remove(client);

        return await context.SaveChangesAsync() > 0;
    }

    public IQueryable<Client> GetAll()
    {
        return context.Clients;
    }

    public async Task<Client> GetByIdAsync(Guid clientId)
    {
        return await context.Clients.FindAsync(clientId);
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        context.Update(client);
        await context.SaveChangesAsync();

        return client;
    }
}
