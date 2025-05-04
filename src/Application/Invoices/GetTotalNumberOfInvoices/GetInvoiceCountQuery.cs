using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Invoices.GetTotalNumberOfInvoices;

public class GetInvoiceCountQuery(
    Guid clientId,
    DateTime startDate,
    DateTime endDate,
    Guid? customerId) : IRequest<int>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;

    public DateTime StartDate { get; set; } = startDate;

    public DateTime EndDate { get; set; } = endDate;

    public Guid? CustomerId { get; set; } = customerId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetInvoiceCountQueryHandler
    (IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    IValidator<GetInvoiceCountQuery> validator)
    : IRequestHandler<GetInvoiceCountQuery, int>
{
    public async Task<int> Handle(GetInvoiceCountQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        if (request.CustomerId.HasValue)
        {
            bool customerExists = await customerRepository
                .GetAll()
                .AnyAsync(c => c.Id == request.CustomerId.Value, cancellationToken);

            if (!customerExists)
            {
                throw new NotFoundException($"Customer with ID {request.CustomerId} not found for this client.");
            }
        }

        var query = invoiceRepository.GetAll()
                    .Where(i => i.IssueDate <= request.EndDate &&
                    i.DueDate >= request.StartDate &&
                    (!request.CustomerId.HasValue || i.CustomerId == request.CustomerId.Value));

        var count = await query.CountAsync(cancellationToken);

        if (count == 0)
        {
            throw new NotFoundException("No Invoices issued in selected period");
        }

        return count;
    }
}
