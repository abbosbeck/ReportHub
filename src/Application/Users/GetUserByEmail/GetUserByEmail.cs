using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Users.GetUserByEmail;

public sealed class GetUserByEmailQuery : IRequest<UserDto>
{
    public string Email { get; init; }
}

[RequiresSystemRole(SystemUserRoles.SystemAdmin)]
public class GetUserByEmailQueryHandler(IMapper mapper, UserManager<User> userManager)
    : IRequestHandler<GetUserByEmailQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException($"User is not found with this email: {request.Email}");

        return mapper.Map<UserDto>(user);
    }
}