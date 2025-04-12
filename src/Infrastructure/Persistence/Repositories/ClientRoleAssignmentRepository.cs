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
}
