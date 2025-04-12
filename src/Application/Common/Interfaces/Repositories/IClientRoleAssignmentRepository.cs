using Application.Common.Services;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IClientRoleAssignmentRepository
{
    Task AddAsync(ClientRoleAssignment clientRoleAssignment);

    Task<List<JwtClientRole>> GetRolesByUserIdAsync(Guid userId);
}
