using Application.Common.Services;

namespace Application.Common.Interfaces.Authorization;

public interface ICurrentUserService
{
    Guid UserId { get; }

    List<string> Roles { get; }

    List<JwtClientRole> ClientRoles();
}