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

    public async Task<List<string>> GetByUserIdAsync(Guid userId)
    {
        return await context.ClientRoleAssignments
                .Where(cr => cr.UserId == userId)
                .Include(cr => cr.ClientRole)
                .Select(cr => cr.ClientRole.Name)
                .Distinct()
                .ToListAsync();
    }

    public async Task<List<string>> GetRolesByUserIdAndClientIdAsync(Guid userId, Guid clientId)
    {
        return await context.ClientRoleAssignments
            .Where(clientRoleAssignment => clientRoleAssignment.ClientId == clientId && clientRoleAssignment.UserId == userId)
            .Select(clientRoleAssignment => clientRoleAssignment.ClientRole.Name)
            .ToListAsync();
    }
}
