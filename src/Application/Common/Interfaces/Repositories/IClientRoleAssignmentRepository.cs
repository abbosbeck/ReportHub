using Application.Common.Configurations;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IClientRoleAssignmentRepository
{
    Task AddAsync(ClientRoleAssignment clientRoleAssignment);

    Task<List<string>> GetRolesByUserIdAndClientIdAsync(Guid userId, Guid clientId);
}
