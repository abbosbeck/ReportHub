using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Users.ConfirmUserEmail;

public sealed class ConfirmUserEmailQuery : IRequest<bool>
{
    public Guid UserId { get; init; }

    public string Token { get; init; }
}

public class ConfirmUserEmailQueryHandler(UserManager<User> userManager)
    : IRequestHandler<ConfirmUserEmailQuery, bool>
{
    public async Task<bool> Handle(ConfirmUserEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new NotFoundException($"User is not found with this id: {request.UserId}");

        var result = await userManager.ConfirmEmailAsync(user, request.Token!);

        if (!result.Succeeded)
        {
            throw new NotFoundException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return result.Succeeded;
    }
}