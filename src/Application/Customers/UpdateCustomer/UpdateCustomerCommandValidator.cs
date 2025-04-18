namespace Application.Customers.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(customer => customer.CountryCode).NotEmpty().MaximumLength(10);

        RuleFor(customer => customer.Email).EmailAddress();

        RuleFor(customer => customer.Name).NotEmpty().MaximumLength(200);
    }
}