using Application.Common.Interfaces;
using Domain.Entity;

namespace Application.Users.RegisterUser
{
    public class RegisterUserCommandHandler(
            IUserRepository repository,
            IValidator<RegisterUserCommandRequest> validator,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper)
            : IRequestHandler<RegisterUserCommandRequest, RegisterUserDto>
        {
            public async Task<RegisterUserDto> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
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
}
