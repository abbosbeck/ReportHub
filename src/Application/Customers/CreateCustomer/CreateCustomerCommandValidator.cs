using Domain.Entities;

namespace Application.Customers.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(customer => customer.CountryCode).NotEmpty().MaximumLength(10);

        RuleFor(customer => customer.Email).EmailAddress();

        RuleFor(customer => customer.Name).NotEmpty().MaximumLength(200);
    }
}