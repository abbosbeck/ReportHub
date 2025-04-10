using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateAccessTokenAsync(User user);

    Task<string> GenerateAccessTokenAsync(Client client);

    string GenerateRefreshToken();

    Task<string> GenerateAndSaveRefreshToken(User user);
}
