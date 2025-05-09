namespace Web.Models.Clients;

public class ClientCreateRequest
{
    public string Name { get; set; }

    public string CountryCode { get; set; }

    public Guid OwnerId { get; set; }
}
