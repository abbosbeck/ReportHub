using Domain.Entities;

namespace Application.Common.Interfaces.Authorization;

public interface IJwtTokenGenerator
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();

    Task<string> GenerateAndSaveRefreshTokenAsync(User user);
}
