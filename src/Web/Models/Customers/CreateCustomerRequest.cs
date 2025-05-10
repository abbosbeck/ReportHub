namespace Web.Models.Customers;

public class CreateCustomerRequest
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string CountryCode { get; set; }
}