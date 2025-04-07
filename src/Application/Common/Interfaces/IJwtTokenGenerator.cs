using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();

    Task<string> GenerateAndSaveRefreshToken(User user);
}
