using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        Task<string> GenerateAndSaveRefreshToken(User user);
    }
}
