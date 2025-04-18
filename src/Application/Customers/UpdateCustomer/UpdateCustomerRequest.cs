namespace Application.Customers.UpdateCustomer;

public class UpdateCustomerRequest
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string CountryCode { get; init; }
}
