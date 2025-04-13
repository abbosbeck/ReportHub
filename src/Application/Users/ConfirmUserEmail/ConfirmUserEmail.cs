using Application.Common.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Linq;

namespace Application.Users.ConfirmUserEmail;

public sealed class ConfirmUserEmailQuery : IRequest<bool>
{
    public Guid UserId { get; init; }

    public string Token { get; init; }
}

public class ConfirmUserEmailQueryHandler(
    IDataProtectionProvider dataProtectionProvider,
    UserManager<User> userManager)
    : IRequestHandler<ConfirmUserEmailQuery, bool>
{
    public async Task<bool> Handle(ConfirmUserEmailQuery request, CancellationToken cancellationToken)
    {
        var dataProtector = dataProtectionProvider.CreateProtector("EmailConfirmation");

        var userId = dataProtector.Unprotect(request.Token);
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException($"User is not found.");

        user.EmailConfirmed = true;
        var updatedUser = await userManager.UpdateAsync(user);

        return updatedUser.Succeeded;
    }
}