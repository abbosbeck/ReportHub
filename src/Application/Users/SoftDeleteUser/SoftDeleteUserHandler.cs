using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Users.SoftDeleteUser
{
    [AllowedFor(UserRoles.Admin)]
    public sealed record SoftDeleteUserCommand(Guid userId) : IRequest<bool>;

    public class SoftDeleteUserHandler(
        IUserRepository userRepository)
        : IRequestHandler<SoftDeleteUserCommand, bool>
    {
        public async Task<bool> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            var isUserDeleted = await userRepository.SoftDeleteUserAsync(request.userId);
            if (!isUserDeleted)
            {
                throw new NotFoundException($"User is not found with this id: {request.userId}");
            }

            return true;
        }
    }
}
