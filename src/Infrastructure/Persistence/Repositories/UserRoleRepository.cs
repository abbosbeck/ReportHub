using Application.Common.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRoleRepository(AppDbContext context) : IUserRoleRepository
    {
        public async Task<IEnumerable<string>> GetUserRolesByUserIdAsync(Guid userId)
        {
            var selectAllUsers = await context.UserRoles.Select(x => x.RoleId).ToListAsync();

            var userRole = await context.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var roles = await context.Roles
                .Where(x => x.Id == userRole.RoleId)
                .Select(x => x.Name)
                .ToListAsync();

            return roles;
        }
    }
}
