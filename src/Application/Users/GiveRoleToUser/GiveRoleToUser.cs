using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Users.GiveRoleToUser
{
    [AllowedFor(UserRoles.Admin)]
    public sealed record GiveRoleToUserCommand(Guid userId, string roleName) : IRequest<bool>;

    public class GiveRoleToUser(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository)
        : IRequestHandler<GiveRoleToUserCommand, bool>
    {
        public async Task<bool> Handle(GiveRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(request.userId)
                ?? throw new NotFoundException($"User is not found with this id: {request.userId}");

            var userRole = new UserRole
            {
                UserId = user.Id,
                Role = new Role
                {
                    Name = request.roleName,
                },
            };

            var addUserRole = await userRoleRepository.GiveRoleToUserAsync(userRole);

            return addUserRole;
        }
    }
}
