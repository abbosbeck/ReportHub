using Domain.Entities;

namespace Application.Customers.CreateCustomer;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(customer => customer.CountryCode).NotEmpty().MaximumLength(10);

        RuleFor(customer => customer.Email).EmailAddress();

        RuleFor(customer => customer.Name).NotEmpty().MaximumLength(200);
    }
}