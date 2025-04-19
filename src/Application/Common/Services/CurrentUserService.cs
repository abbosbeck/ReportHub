using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    public Guid UserId => httpContextAccessor.HttpContext?.User.GetUserId() ?? Guid.Empty;

    public List<string> SystemRoles => httpContextAccessor.HttpContext?.User.GetRoles() ?? new List<string> { };
}