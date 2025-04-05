using Application.Common.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRoleRepository(AppDbContext context) : IUserRoleRepository
    {
        public async Task<string> GetUserRolesByUserIdAsync(Guid userId)
        {
            var userRole = await context.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var role = await context.Roles
                .FirstOrDefaultAsync(x => x.Id == userRole.RoleId);

            return role.Name;
        }
    }
}
