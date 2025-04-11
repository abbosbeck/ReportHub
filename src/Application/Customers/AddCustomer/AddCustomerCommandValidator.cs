namespace Application.Customers.AddCustomer;

public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    public AddCustomerCommandValidator()
    {
        RuleFor(customer => customer.Name).NotEmpty();
        RuleFor(customer => customer.Country).NotEmpty();
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress();
    }
}