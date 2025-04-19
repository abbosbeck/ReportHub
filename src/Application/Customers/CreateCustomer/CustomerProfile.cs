using Domain.Entities;

namespace Application.Customers.CreateCustomer;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerRequest, Customer>();
    }
}