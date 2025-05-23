﻿using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class SystemRoleAssignmentRepository(AppDbContext context) : ISystemRoleAssignmentRepository
{
    public async Task<bool> AssignRoleToUserAsync(SystemRoleAssignment systemRoleAssignment)
    {
        await context.AddAsync(systemRoleAssignment);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<string>> GetRolesByUserIdAsync(Guid userId)
    {
        var roles = await context.UserRoles.Where(r => r.UserId == userId)
            .Join(
                context.Roles,
                sr => sr.RoleId,
                role => role.Id,
                (sr, role) => role.Name)
            .ToListAsync();

        return roles;
    }
}
