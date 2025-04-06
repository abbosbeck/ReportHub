namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }

    List<string> Roles { get; }
}