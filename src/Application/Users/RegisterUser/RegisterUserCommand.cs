
using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.AspNet.Identity;

namespace Application.Users.RegisterUser
{
    public class RegisterUserCommandRequest : IRequest<RegisterUserDto>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public class RegisterUserCommandHandler(
            IUserRepository repository,
            IValidator<RegisterUserCommandRequest> validator,
            IPasswordHasher passwordHasher,
            IMapper mapper)
            : IRequestHandler<RegisterUserCommandRequest, RegisterUserDto>
        {
            public async Task<RegisterUserDto> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
            {
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                var hashedPassword = passwordHasher.HashPassword(request.Password);

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Department = request.Department,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = hashedPassword,
                };

                await repository.AddUser(user);

                var result = mapper.Map<RegisterUserDto>(user);

                return result;
            }
        }
    }
}
