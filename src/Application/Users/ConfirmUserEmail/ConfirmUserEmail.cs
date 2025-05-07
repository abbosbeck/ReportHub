using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection;

namespace Application.Users.ConfirmUserEmail;

public sealed class ConfirmUserEmailQuery(string token) : IRequest<string>
{
    public string Token { get; init; } = token;
}

public class ConfirmUserEmailQueryHandler(
    IDataProtectionProvider dataProtectionProvider,
    IUserRepository repository)
    : IRequestHandler<ConfirmUserEmailQuery, string>
{
    public async Task<string> Handle(ConfirmUserEmailQuery request, CancellationToken cancellationToken)
    {
        var dataProtector = dataProtectionProvider.CreateProtector("EmailConfirmation");

        var userId = dataProtector.Unprotect(request.Token);
        var user = await repository.GetByIdAsync(Guid.Parse(userId))
            ?? throw new NotFoundException($"User is not found.");

        user.EmailConfirmed = true;
        await repository.UpdateAsync(user);

        return "Welcome to ReportHub!";
    }
}