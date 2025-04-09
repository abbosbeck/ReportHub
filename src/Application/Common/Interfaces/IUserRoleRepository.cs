using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<bool> GiveRoleToUserAsync(SystemRoleAssignment userRole);

        Task<List<string>> GetUserRolesByUserIdAsync(Guid userId);
    }
}
