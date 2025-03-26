using Domain;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByName(string firstName);
}
