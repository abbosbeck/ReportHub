using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Users.SoftDeleteUser;

public sealed class SoftDeleteUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}

[RequiresSystemRole(SystemUserRoles.SystemAdmin)]
public class SoftDeleteUserCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<SoftDeleteUserCommand, bool>
{
    public async Task<bool> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
    {
        var isUserDeleted = await userRepository.SoftDeleteUserAsync(request.UserId);
        if (!isUserDeleted)
        {
            throw new NotFoundException($"User is not found with this id: {request.UserId}");
        }

        return true;
    }
}
