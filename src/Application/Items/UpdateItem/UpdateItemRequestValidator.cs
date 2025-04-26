namespace Application.Items.UpdateItem;

public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
{
    public UpdateItemRequestValidator()
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
    }
}
