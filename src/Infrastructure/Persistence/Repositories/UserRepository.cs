using Application.Common.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task AddUser(User user)
    {
        await context.Set<User>().AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByName(string firstName)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => EF.Functions.ILike(u.FirstName, firstName));
    }

    public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => EF.Functions.ILike(u.PhoneNumber!, phoneNumber));
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => EF.Functions.ILike(u.RefreshToken, refreshToken));
    }

    public async Task SaveChanges()
    {
         await context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}