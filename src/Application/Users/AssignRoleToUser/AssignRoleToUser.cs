using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Users.AssignRoleToUser;

public sealed class AssignRoleToUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string RoleName { get; set; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class AssignRoleToUserCommandHandler(
    IUserRepository userRepository,
    ISystemRoleAssignmentRepository systemRoleAssignmentRepository,
    IValidator<AssignRoleToUserCommand> validator)
    : IRequestHandler<AssignRoleToUserCommand, bool>
{
    public async Task<bool> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User is not found with this id: {request.UserId}");

        var userRole = new SystemRoleAssignment
        {
            UserId = user.Id,
            Role = new SystemRole
            {
                Name = request.RoleName,
            },
        };

        var succeed = await systemRoleAssignmentRepository.AssignRoleToUserAsync(userRole);

        return succeed;
    }
}
