using Application.Common.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class ClientRoleAssignmentRepository(AppDbContext dbContext) : IClientRoleAssignmentRepository
{
    public async Task<List<string>> GetClientRolesByClientIdAsync(Guid clientId)
    {
        return await dbContext.ClientRoleAssignment
            .Where(t => t.ClientId == clientId)
            .Select(t => t.ClientRole.Name)
            .ToListAsync();
    }
}