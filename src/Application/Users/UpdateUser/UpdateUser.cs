using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Users.UpdateUser;

public class UpdateUserCommand : IRequest<UserDto>
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Department { get; set; }

    public string Password { get; set; }
}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IValidator<UpdateUserCommand> validator)
    : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        var user = await userRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"User is not found with this id: {request.Id}");

        mapper.Map(request, user);
        await userRepository.UpdateAsync(user);

        return mapper.Map<UserDto>(user);
    }
}
