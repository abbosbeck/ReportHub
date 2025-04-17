using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IInvoiceRepository
{
    Task<Invoice> AddAsync(Invoice invoice);

    Task<Invoice> GetByIdAsync(Guid invoiceId);

    Task<Invoice> UpdateAsync(Invoice invoice);

    Task<IEnumerable<Invoice>> GetAllAsync();

    Task<bool> DeleteAsync(Invoice invoice);
}
