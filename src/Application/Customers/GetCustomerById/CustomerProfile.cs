using Domain.Entities;

namespace Application.Customers.GetCustomerById;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}