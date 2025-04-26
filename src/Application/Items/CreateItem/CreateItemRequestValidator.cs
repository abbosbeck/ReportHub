using Application.Common.ExtensionMethods;

namespace Application.Items.CreateItem;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3);

        RuleFor(c => c.InvoiceId)
            .NotEmpty()
            .BeAValidGuid();
    }
}
