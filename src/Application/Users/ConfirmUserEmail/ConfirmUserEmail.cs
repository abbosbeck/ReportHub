using Application.Common.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection;

namespace Application.Users.ConfirmUserEmail;

public sealed class ConfirmUserEmailQuery(string token) : IRequest<string>
{
    public string Token { get; init; } = token;
}

public class ConfirmUserEmailQueryHandler(
    IDataProtectionProvider dataProtectionProvider,
    UserManager<User> userManager)
    : IRequestHandler<ConfirmUserEmailQuery, string>
{
    public async Task<string> Handle(ConfirmUserEmailQuery request, CancellationToken cancellationToken)
    {
        var dataProtector = dataProtectionProvider.CreateProtector("EmailConfirmation");

        var userId = dataProtector.Unprotect(request.Token);
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException($"User is not found.");

        user.EmailConfirmed = true;
        await userManager.UpdateAsync(user);

        return "Welcome to ReportHub!";
    }
}