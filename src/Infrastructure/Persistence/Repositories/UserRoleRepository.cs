using Application.Common.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRoleRepository(AppDbContext context) : IUserRoleRepository
    {
        public async Task<bool> GiveRoleToUserAsync(UserRole userRole)
        {
            var role = await context.Set<Role>()
                .FirstOrDefaultAsync(x => x.Name == userRole.Role.Name);
            if (role == null)
            {
                return false;
            }

            userRole.RoleId = role.Id;
            await context.Set<UserRole>().AddAsync(userRole);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string?>> GetUserRolesByUserIdAsync(Guid userId)
        {
            var userRoles = await context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.Role.Name)
                .ToListAsync();

            return userRoles;
        }
    }
}
