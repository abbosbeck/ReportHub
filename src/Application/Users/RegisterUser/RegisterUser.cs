using Application.Common.Attributes;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.RegisterUser;

[AllowedFor]
public sealed class RegisterUserQuery : IRequest<RegisterUserDto>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Department { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }
}

public class RegisterUserQueryHandler(
        IUserRepository repository,
        IValidator<RegisterUserQuery> validator,
        IPasswordHasher<User> passwordHasher,
        IMapper mapper)
        : IRequestHandler<RegisterUserQuery, RegisterUserDto>
{
        public async Task<RegisterUserDto> Handle(RegisterUserQuery request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Department = request.Department,
                PhoneNumber = request.PhoneNumber,
            };

            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

            await repository.AddUser(user);

            var result = mapper.Map<RegisterUserDto>(user);

            return result;
        }
}
