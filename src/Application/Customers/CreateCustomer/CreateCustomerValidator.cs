namespace Application.Customers.CreateCustomer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Please provide valid name!");

        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200).
            WithMessage("Please provide valid email!");

        RuleFor(c => c.Country)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Please provide valid Country!");
    }
}