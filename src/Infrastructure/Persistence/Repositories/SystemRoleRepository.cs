using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class SystemRoleRepository(AppDbContext context) : ISystemRoleRepository
{
    public async Task<SystemRole> GetByNameAsync(string name)
    {
        return await context.Roles.
            FirstOrDefaultAsync(sr => sr.Name == name);
    }
}
