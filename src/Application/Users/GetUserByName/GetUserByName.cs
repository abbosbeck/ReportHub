using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

namespace Application.Users.GetUserByName;

public sealed class GetUserByNameQuery : IRequest<UserDto>
{
    public string FirstName { get; init; } = string.Empty;
}

[RequiresSystemRole(SystemUserRoles.SystemAdmin)]
public class GetUserByNameQueryHandler(
    IUserRepository repository,
    IValidator<GetUserByNameQuery> validator,
    IMapper mapper)
    : IRequestHandler<GetUserByNameQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await repository.GetUserByName(request.FirstName)
            ?? throw new NotFoundException("User is not found");

        var result = mapper.Map<UserDto>(user);

        return result;
    }
}
