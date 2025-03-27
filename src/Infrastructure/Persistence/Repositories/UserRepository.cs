using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByName(string firstName)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => EF.Functions.ILike(u.FirstName, firstName));
    }
}
