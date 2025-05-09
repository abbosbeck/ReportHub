using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User> AddAsync(User user)
    {
        await context.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> DeleteAsync(User user)
    {
        context.Remove(user);

        return await context.SaveChangesAsync() > 0;
    }

    public IQueryable<User> GetAll()
    {
        return context.Users.AsQueryable();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        return await context.Users.FindAsync(userId);
    }

    public async Task<User> GetByRefreshTokenAsync(string refreshToken)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<User> UpdateAsync(User user)
    {
        context.Update(user);
        await context.SaveChangesAsync();

        return user;
    }
}