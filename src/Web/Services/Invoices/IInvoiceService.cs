using Web.Models.Invoices;

namespace Web.Services.Invoices;

public interface IInvoiceService
{
    Task<List<InvoiceResponse>> GetAllAsync(Guid clientId);

    Task<InvoiceResponse> GetByIdAsync(Guid id, Guid clientId);

    Task<bool> CreateAsync(InvoiceCreateRequest invoice, Guid clienId);

    Task<bool> DeleteAsync(Guid id, Guid clientId);

    Task<byte[]> DownloadInvoiceAsync(Guid id, Guid clientId);
}
