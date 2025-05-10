namespace Web.Models.Customers;

public class CustomerResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string CountryCode { get; set; }
}
