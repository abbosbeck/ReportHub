using System.Web;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
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
        UserManager<User> userManager,
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

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new UnauthorizedException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await AssignSystemRoleAsync(user, SystemRoles.Regular);

        var dataProtector = dataProtectorTokenProvider.CreateProtector("EmailConfirmation");
        var userId = user.Id.ToString();
        var token = dataProtector.Protect(userId);

        var confirmationUrl = $"{configuration["AppUrl"]}/api/users/confirm-email?token={token}";

        await emailService.SendEmailAsync(
            user.Email,
            "Email confirmation!",
            $@"<h1>Welcome to ReportHub</h1>Please confirm your account by clicking <a href='{confirmationUrl}'>here</a>");

        return mapper.Map<UserDto>(user);
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
}