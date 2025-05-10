using Web.Models.Invoices;

namespace Web.Services.Invoices;

public interface IInvoiceService
{
    Task<List<InvoiceResponse>> GetAllAsync(Guid clientId);

    Task<InvoiceResponse> GetByIdAsync(Guid id, Guid clientId);
}
