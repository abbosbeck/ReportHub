using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ClientRoleRepository(AppDbContext context) : IClientRoleRepository
{
    public async Task<ClientRole> GetByNameAsync(string name)
    {
        return await context.ClientRoles.FirstOrDefaultAsync(cr => cr.Name == name);
    }
}
