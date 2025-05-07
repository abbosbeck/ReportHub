using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        customers.ForEach(customer =>
        {
            if (customer.ClientId != request.ClientId)
            {
                throw new ForbiddenException("Givin ClientId(s) is invalid. Please check and try again!");
            }
        });

        var oldCustomers = await repository.GetAll()
            .ToListAsync(cancellationToken);

        customers = customers
            .Where(newCust => !oldCustomers.Any(existing =>
                existing.Name == newCust.Name &&
                existing.Email == newCust.Email &&
                existing.CountryCode == newCust.CountryCode))
            .ToList();

        await repository.AddBulkAsync(customers);

        return mapper.Map<List<CustomerDto>>(customers);
    }
}
