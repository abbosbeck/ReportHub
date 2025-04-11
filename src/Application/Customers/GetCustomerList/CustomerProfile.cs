using Domain.Entities;

namespace Application.Customers.GetCustomerList;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}