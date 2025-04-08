using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<bool> GiveRoleToUserAsync(UserSystemRole userRole);

        Task<List<string>> GetUserRolesByUserIdAsync(Guid userId);
    }
}
