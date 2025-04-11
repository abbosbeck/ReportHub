using Application.Common.Constants;
using Application.Common.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class ClientRoleAssignmentRepository(AppDbContext dbContext) : IClientRoleAssignmentRepository
{
    public async Task<List<string>> GetClientRolesByClientIdAsync(Guid clientId)
    {
        var roles = await dbContext.ClientRoleAssignment
            .Where(t => t.ClientId == clientId)
            .Select(t => t.ClientRole.Name)
            .ToListAsync();
        if (roles is null or[])
        {
            roles.Add(ClientUserRoles.Regular);
        }

        return roles;
    }
}