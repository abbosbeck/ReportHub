using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ISystemRoleAssignmentRepository
{
    Task<bool> AssignRoleToUserAsync(SystemRoleAssignment systemRoleAssignment);

    Task<List<string>> GetRolesByUserIdAsync(Guid userId);
}
