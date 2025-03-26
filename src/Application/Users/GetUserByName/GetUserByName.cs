using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Users.GetUserByName;

public class GetUserByNameRequest : IRequest<UserDto>
{
    public string FirstName { get; set; } = string.Empty;
}

public class GetUserByNameRequestHandler(IUserRepository repository, IValidator<GetUserByNameRequest> validator, IMapper mapper)
    : IRequestHandler<GetUserByNameRequest, UserDto>
{
    public async Task<UserDto> Handle(GetUserByNameRequest request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await repository.GetUserByName(request.FirstName)
            ?? throw new NullReferenceException("User not found with this name");

        var result = mapper.Map<UserDto>(user);

        return result;
    }
}
