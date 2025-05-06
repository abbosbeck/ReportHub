using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Customers.ImportCustomerList;

public class ImportCustomerListCommand(
    Guid clientId,
    IFormFile customersData)
    : IRequest<List<CustomerDto>>, IClientRequest
{
    public IFormFile CustomersData { get; set; } = customersData;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class ImportCustomerListCommandHandler(
    IImportDataFromFileService importDataFromFileService,
    ICustomerRepository repository,
    IMapper mapper)
    : IRequestHandler<ImportCustomerListCommand, List<CustomerDto>>
{
    public async Task<List<CustomerDto>> Handle(ImportCustomerListCommand request, CancellationToken cancellationToken)
    {
        var customers = importDataFromFileService
            .ImportCustomerListFromExcel(request.CustomersData);

        await repository.AddBulkAsync(customers);

        return mapper.Map<List<CustomerDto>>(customers);
    }
}
