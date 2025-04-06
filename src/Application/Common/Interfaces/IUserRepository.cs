using Domain.Entity;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByName(string firstName);

    Task<User> GetUserByIdAsync(Guid id);

    Task AddUser(User user);

    Task<User> GetUserByPhoneNumberAsync(string phoneNumber);

    Task UpdateUserAsync(User user);

    Task SaveChanges();

    Task<User> GetUserByRefreshTokenAsync(string refreshToken);

    Task<bool> SoftDeleteUserAsync(Guid userId);
}
