using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class SystemRoleAssignmentRepository(AppDbContext context) : ISystemRoleAssignmentRepository
{
    public async Task<bool> AssignRoleToUserAsync(SystemRoleAssignment systemRoleAssignment)
    {
        await context.AddAsync(systemRoleAssignment);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<string>> GetByNameAsync(Guid userId)
    {
        var roles = await context.UserRoles
            .Where(r => r.UserId == userId)
            .Select(r => r.Role.Name)
            .ToListAsync();

        return roles;
    }
}
