using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Users.AssignSystemRole;

public sealed class AssignSystemRoleCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string RoleName { get; set; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class AssignSystemRoleCommandHandler(
    IUserRepository userRepository,
    ISystemRoleAssignmentRepository systemRoleAssignmentRepository,
    ISystemRoleRepository systemRoleRepository,
    IValidator<AssignSystemRoleCommand> validator)
    : IRequestHandler<AssignSystemRoleCommand, bool>
{
    public async Task<bool> Handle(AssignSystemRoleCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User is not found with this id: {request.UserId}");

        var systemRole = await systemRoleRepository.GetByNameAsync(request.RoleName)
            ?? throw new NotFoundException($"System role is not found with this name: {request.RoleName}");

        var userRole = new SystemRoleAssignment
        {
            UserId = user.Id,
            RoleId = systemRole.Id,
        };

        var succeed = await systemRoleAssignmentRepository.AssignRoleToUserAsync(userRole);

        return succeed;
    }
}
