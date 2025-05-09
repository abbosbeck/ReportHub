namespace Application.Common.Interfaces.Authorization;

public interface ICurrentUserService
{
    Guid UserId { get; }

    List<string> SystemRoles();
}