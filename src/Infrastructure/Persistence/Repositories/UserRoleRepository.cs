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

        public async Task<string?> GetUserRolesByUserIdAsync(Guid userId)
        {
            var userRole = await context.Set<UserRole>()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole == null)
            {
                return null;
            }

            var role = await context.Set<Role>()
                .FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
            if (role == null)
            {
                return null;
            }

            return role.Name;
        }
    }
}
