using Application.Common.Interfaces;

namespace Application.Users.SoftDeleteUser
{
    public sealed record SoftDeleteUserCommand(Guid userId) : IRequest<bool>;

    public class SoftDeleteUserHandler(
        IUserRepository userRepository)
        : IRequestHandler<SoftDeleteUserCommand, bool>
    {
        public async Task<bool> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            var isUserDeleted = await userRepository.SoftDeleteUserAsync(request.userId);

            return isUserDeleted;
        }
    }
}
