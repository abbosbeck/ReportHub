using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace Application.Users.RegisterUser;

public sealed class RegisterUserCommand : IRequest<UserDto>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Department { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}

public class RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IValidator<RegisterUserCommand> validator,
        IConfiguration configuration,
        IEmailService emailService,
        ISystemRoleAssignmentRepository systemRoleAssignmentRepository,
        ISystemRoleRepository systemRoleRepository,
        IDataProtectionProvider dataProtectorTokenProvider,
        IMapper mapper)
        : IRequestHandler<RegisterUserCommand, UserDto>
{
    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = mapper.Map<User>(request);
        user.UserName = user.Email;
        user.NormalizedEmail = user.Email.ToUpper();
        user.SecurityStamp = Guid.NewGuid().ToString();
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        var existUser = await userRepository.GetByEmailAsync(request.Email);

        if (existUser is not null)
        {
            throw new ConflictException("Email Already Exists");
        }

        _ = await userRepository.AddAsync(user);

        await AssignSystemRoleAsync(user, SystemRoles.Regular);

        var dataProtector = dataProtectorTokenProvider.CreateProtector("EmailConfirmation");
        var userId = user.Id.ToString();
        var token = dataProtector.Protect(userId);

        var confirmationUrl = $"{configuration["AppUrl"]}/users/confirm-email?token={token}";

        var emailBody = EmailMessage(confirmationUrl, user.FirstName);

        await emailService.SendEmailAsync(user.Email, "Email confirmation!", emailBody);

        var userDto = mapper.Map<UserDto>(user);
        userDto.ConfirmEmailToken = token;

        return userDto;
    }

    private async Task AssignSystemRoleAsync(
        User user, string roleName)
    {
        var systemRole = await systemRoleRepository.GetByNameAsync(roleName);
        var systemRoleAssignment = new SystemRoleAssignment
        {
            UserId = user.Id,
            RoleId = systemRole.Id,
        };
        await systemRoleAssignmentRepository.AssignRoleToUserAsync(systemRoleAssignment);
    }

    private static string EmailMessage(string confirmationUrl, string firstName)
    {
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "confirm-email.html");
        string emailBody = File.ReadAllText(templatePath);

        emailBody = emailBody.Replace("{{FirstName}}", firstName);
        emailBody = emailBody.Replace("{{ConfirmationLink}}", confirmationUrl);

        return emailBody;
    }
}