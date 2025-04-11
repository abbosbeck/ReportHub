namespace Application.Customers.GetCustomerList;

public class CustomerDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Country { get; init; }

    public Guid ClientId { get; init; }
}