using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Users.SoftDeleteUser;

[AllowedFor(UserRoles.Admin)]
public sealed class SoftDeleteUserQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
}

public class SoftDeleteUserQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<SoftDeleteUserQuery, bool>
{
    public async Task<bool> Handle(SoftDeleteUserQuery request, CancellationToken cancellationToken)
    {
        var isUserDeleted = await userRepository.SoftDeleteUserAsync(request.UserId);
        if (!isUserDeleted)
        {
            throw new NotFoundException($"User is not found with this id: {request.UserId}");
        }

        return true;
    }
}
