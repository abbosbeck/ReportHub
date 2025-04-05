using Application.Common.Interfaces;
using Domain.Entity;

namespace Application.Users.GiveRoleToUser
{
    public sealed record GiveRoleToUserCommand(Guid userId, string roleName) : IRequest<bool>;

    public class GiveRoleToUserHandler(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository)
        : IRequestHandler<GiveRoleToUserCommand, bool>
    {
        public async Task<bool> Handle(GiveRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(request.userId);
            if (user == null)
            {
                throw new ArgumentNullException("User not found!");
            }

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
