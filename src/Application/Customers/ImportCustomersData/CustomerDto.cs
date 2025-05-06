namespace Application.Customers.ImportCustomersData;

public class CustomerDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string CountryCode { get; init; }

    public Guid ClientId { get; init; }
}