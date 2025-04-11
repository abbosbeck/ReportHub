using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid userId);

    Task<User> AddAsync(User user);

    Task<User> UpdateAsync(User user);

    Task<User> GetByRefreshTokenAsync(string refreshToken);

    Task<bool> DeleteAsync(User user);
}
