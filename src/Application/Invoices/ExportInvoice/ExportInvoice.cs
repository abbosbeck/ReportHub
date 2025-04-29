using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;
using Application.Invoices.ExportInvoice.Document;
using Domain.Entities;
using Domain.Enums;
using QuestPDF.Fluent;

namespace Application.Invoices.ExportInvoice;

public class ExportInvoiceCommand(Guid invoiceId, Guid clientId) : IRequest<ExportPdfDto>, IClientRequest
{
    public Guid InvoiceId { get; init; } = invoiceId;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin, ClientRoles.Operator)]
public class ExportInvoiceCommandHandler(
    ILogRepository logRepository,
    IDateTimeService dateTimeService,
    IInvoiceRepository invoiceRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<ExportInvoiceCommand, ExportPdfDto>
{
    public async Task<ExportPdfDto> Handle(ExportInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByIdAsync(request.InvoiceId)
            ?? throw new NotFoundException($"Invoice is not found with this Id: {request.InvoiceId}");
        var log = new Log
        {
            ClientId = request.ClientId,
            InvoiceId = request.InvoiceId,
            UserId = currentUserService.UserId,
            TimeStamp = dateTimeService.UtcNow,
        };
        byte[] pdfBytes;

        try
        {
            var document = new InvoiceDocument(invoice);
            pdfBytes = document.GeneratePdf();
            log.Status = LogStatus.Success;
            await logRepository.AddAsync(log);
        }
        catch
        {
            log.Status = LogStatus.Failure;
            await logRepository.AddAsync(log);
            throw;
        }

        return new ExportPdfDto
        {
            ByteArray = pdfBytes,
            FileName = $"{invoice.InvoiceNumber}_invoice.pdf",
            ContentType = "application/pdf",
        };
    }
}