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
public class GetInvoiceCountQueryHandler(
    IInvoiceRepository invoiceRepository,
    IValidator<GetInvoiceCountQuery> validator)
    : IRequestHandler<GetInvoiceCountQuery, int>
{
    public async Task<int> Handle(GetInvoiceCountQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var query = invoiceRepository
            .GetAll()
            .Where(invoice => invoice.IssueDate <= request.EndDate && invoice.DueDate >= request.StartDate &&
                              (!request.CustomerId.HasValue || invoice.CustomerId == request.CustomerId.Value));

        var count = await query.CountAsync(cancellationToken);

        if (count == 0)
        {
            throw new NotFoundException("No Invoices issued in selected period");
        }

        return count;
    }
}
