using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetUserList;

public class GetUserListQuery : IRequest<List<UserDto>>
{

}

[RequiresSystemRole(SystemRoles.SuperAdmin)]
public class GetUserListQueryHandler(
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<GetUserListQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAll()
            .Where(u => u.Email != "admin@gmail.com")
            .ToListAsync(cancellationToken);

        return mapper.Map<List<UserDto>>(users);
    }
}
