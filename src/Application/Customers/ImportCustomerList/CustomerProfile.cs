using Domain.Entities;

namespace Application.Customers.ImportCustomerList;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}
