using Domain.Entities;

namespace Application.Customers.AddCustomer;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<AddCustomerCommand, Customer>();
        CreateMap<Customer, CustomerDto>();
    }
}