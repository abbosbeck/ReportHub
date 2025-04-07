using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Users.GiveRoleToUser;

[AllowedFor(UserRoles.Admin)]
public sealed class GiveRoleToUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string RoleName { get; set; }
}

public class GiveRoleToUserCommandHandler(
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository)
    : IRequestHandler<GiveRoleToUserCommand, bool>
{
    public async Task<bool> Handle(GiveRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User is not found with this id: {request.UserId}");

        var userRole = new UserRole
        {
            UserId = user.Id,
            Role = new Role
            {
                Name = request.RoleName,
            },
        };

        var addUserRole = await userRoleRepository.GiveRoleToUserAsync(userRole);

        return addUserRole;
    }
}
