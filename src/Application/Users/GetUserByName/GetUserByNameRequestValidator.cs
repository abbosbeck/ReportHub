using FluentValidation;

namespace Application.Users.GetUserByName;

public class GetUserByNameRequestValidator : AbstractValidator<GetUserByNameRequest>
{
    public GetUserByNameRequestValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty()
            .MaximumLength(64);
    }
}