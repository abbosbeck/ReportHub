using Application.Common.Interfaces;

namespace Application.Customers.GetCustomerList;

public class GetCustomerListQuery : IRequest<IEnumerable<CustomerDto>>
{
}

public class GetCustomerListQueryHandler(IMapper mapper, ICustomerRepository repository)
    : IRequestHandler<GetCustomerListQuery, IEnumerable<CustomerDto>>
{
    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var customers = await repository.GetAllCustomer();
        return mapper.Map<IEnumerable<CustomerDto>>(customers);
    }
}