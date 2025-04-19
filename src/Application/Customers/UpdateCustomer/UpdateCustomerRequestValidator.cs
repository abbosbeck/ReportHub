namespace Application.Customers.UpdateCustomer;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(customer => customer.CountryCode).NotEmpty().MaximumLength(10);

        RuleFor(customer => customer.Email).EmailAddress();

        RuleFor(customer => customer.Name).NotEmpty().MaximumLength(200);
    }
}