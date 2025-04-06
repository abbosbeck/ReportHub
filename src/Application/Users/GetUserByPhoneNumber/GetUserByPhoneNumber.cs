using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Users.GetUserByPhoneNumber;

public class GetUserByPhoneNumberQuery : IRequest<UserDto>
{
    public string PhoneNumber { get; init; }
}

public class GetUserByPhoneNumberQueryHandler(IMapper mapper, IUserRepository userRepository)
    : IRequestHandler<GetUserByPhoneNumberQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByPhoneNumberQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber)
            ?? throw new NotFoundException($"User is not found with this phone number: {request.PhoneNumber}");

        return mapper.Map<UserDto>(user);
    }
}