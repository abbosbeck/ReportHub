using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Customers.ImportCustomersData;

public class ImportCustomersDataCommand(
    Guid clientId,
    IFormFile customersData)
    : IRequest<List<CustomerDto>>, IClientRequest
{
    public IFormFile CustomersData { get; set; } = customersData;

    public Guid ClientId { get; set; } = clientId;
}

public class ImportCustomersDataCommandHandler(
    IImportDataFromFileService importDataFromFileService,
    ICustomerRepository repository,
    IMapper mapper)
    : IRequestHandler<ImportCustomersDataCommand, List<CustomerDto>>
{
    public async Task<List<CustomerDto>> Handle(ImportCustomersDataCommand request, CancellationToken cancellationToken)
    {
        var customers = importDataFromFileService
            .ImportCustomersDataFromExcel(request.CustomersData);

        await repository.AddBulkAsync(customers);

        return mapper.Map<List<CustomerDto>>(customers);
    }
}
