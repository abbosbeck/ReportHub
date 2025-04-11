using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;

namespace Application.Users.DeleteUser;

public sealed class DeleteUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class DeleteUserCommandHandler(
    IUserRepository repository)
    : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("There is no user with this Id!");

        var result = await repository.DeleteAsync(user);

        return result;
    }
}
