using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<bool> GiveRoleToUserAsync(UserRole userRole);

        Task<string> GetUserRolesByUserIdAsync(Guid userId);
    }
}
