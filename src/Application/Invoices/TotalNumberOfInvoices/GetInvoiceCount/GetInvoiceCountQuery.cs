using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Invoices.TotalNumberOfInvoices.GetInvoiceCount;

public class GetInvoiceCountQuery(Guid clientId) : IRequest<int>, IClientRequest
{
    public Guid ClientId { get; set; } = clientId;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid? CustomerId { get; set; }
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class GetInvoiceCountQueryHandler
    (IInvoiceRepository repository,
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
                .AnyAsync(c => c.Id == request.CustomerId.Value && c.ClientId == request.ClientId, cancellationToken);

            if (!customerExists)
            {
                throw new NotFoundException($"Customer with ID {request.CustomerId} not found for this client.");
            }
        }

        var query = repository.GetAll()
            .Where(i => i.ClientId == request.ClientId &&
           i.IssueDate <= request.EndDate &&
           i.DueDate >= request.StartDate);

        if (request.CustomerId.HasValue)
        {
            query = query.Where(i => i.CustomerId == request.CustomerId.Value);
        }

        var count = await query.CountAsync(cancellationToken);

        if (count == 0)
        {
            throw new NotFoundException("No Invoices issued in selected period");
        }

        return count;
    }
}
