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

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    Mapper mapper,
    IValidator<UpdateUserCommand> validator)
    : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _ = await userRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"User is not found with this id: {request.Id}");

        var newUser = mapper.Map<User>(request);
        var updatedUser = await userRepository.UpdateAsync(newUser);

        return mapper.Map<UserDto>(updatedUser);
    }
}
