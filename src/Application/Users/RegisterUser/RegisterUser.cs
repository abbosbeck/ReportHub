using Application.Common.Attributes;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Users.RegisterUser;

[AllowedFor]
public sealed class RegisterUserCommand : IRequest<RegisterUserDto>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Department { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }
}

public class RegisterUserCommandHandler(
        IValidator<RegisterUserCommand> validator,
        UserManager<User> userManager,
        IConfiguration configuration,
        IEmailSender emailSender,
        IMapper mapper)
        : IRequestHandler<RegisterUserCommand, RegisterUserDto>
{
    public async Task<RegisterUserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var existUser = await repository.GetUserByPhoneNumberAsync(request.PhoneNumber);
        if (existUser is not null)
        {
            throw new ConflictException("A user with this phone number already exists.");
        }

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
