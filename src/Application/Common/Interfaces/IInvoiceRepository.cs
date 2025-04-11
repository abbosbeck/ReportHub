using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddAsync(Invoice invoice);

        Task<Invoice> GetByIdAsync(Guid invoiceId);

        Task<IEnumerable<Invoice>> GetAllAsync();

        Task<IEnumerable<Invoice>> GetByClientIdAsync(Guid clientId);

        Task<bool> DeleteAsync(Guid invoiceId);
    }
}
