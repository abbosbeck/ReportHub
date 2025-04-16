using Application.Common.Configurations;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ClientRoleAssignmentRepository(AppDbContext context) : IClientRoleAssignmentRepository
{
    public async Task AddAsync(ClientRoleAssignment clientRoleAssignment)
    {
        await context.AddAsync(clientRoleAssignment);
        await context.SaveChangesAsync();
    }

    public async Task<List<ResolvedClientRole>> GetRolesByUserIdAsync(Guid userId)
    {
        var roles = await context.ClientRoleAssignments
            .Where(r => r.UserId == userId)
            .Select(r => new ResolvedClientRole
            {
                ClientId = r.ClientId,
                RoleName = r.ClientRole.Name,
            })
            .ToListAsync();

        return roles;
    }
}
