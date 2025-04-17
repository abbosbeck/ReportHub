using Domain.Entities;

namespace Application.Customers.UpdateCustomer;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CreateCustomer.CustomerDto>();
        CreateMap<UpdateCustomerCommand, Customer>();
    }
}