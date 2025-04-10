using System.Text;
using Application.Common.Attributes;
using Application.Common.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.Users.ConfirmUserEmail;

public sealed class ConfirmUserEmailQuery : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string Token { get; set; }
}

public class ConfirmUserEmailQueryHandler(UserManager<User> userManager)
    : IRequestHandler<ConfirmUserEmailQuery, bool>
{
    public async Task<bool> Handle(ConfirmUserEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new NotFoundException($"User is not found with this id: {request.UserId}");

        var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
        var emailConfirmationToken = Encoding.UTF8.GetString(decodedToken);
        var result = await userManager.ConfirmEmailAsync(user, emailConfirmationToken);

        if (!result.Succeeded)
        {
            throw new NotFoundException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return result.Succeeded;
    }
}