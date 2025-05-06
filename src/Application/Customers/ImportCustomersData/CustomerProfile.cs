using Domain.Entities;

namespace Application.Customers.ImportCustomersData;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}
