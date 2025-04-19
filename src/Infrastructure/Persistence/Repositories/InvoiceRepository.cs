using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class InvoiceRepository(AppDbContext context) : IInvoiceRepository
{
    public async Task<Invoice> AddAsync(Invoice invoice)
    {
        await context.Invoices.AddAsync(invoice);
        await context.SaveChangesAsync();

        return invoice;
    }

    public async Task<bool> DeleteAsync(Invoice invoice)
    {
        context.Remove(invoice);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        return await context.Invoices.ToListAsync();
    }

    public async Task<Invoice> GetByIdAsync(Guid invoiceId)
    {
        return await context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == invoiceId);
    }

    public async Task<Invoice> GetByInvoiceNumberAsync(string number)
    {
        return await context.Invoices.FirstOrDefaultAsync(i => i.InvoiceNumber == number);
    }

    public async Task<Invoice> UpdateAsync(Invoice invoice)
    {
        context.Update(invoice);
        await context.SaveChangesAsync();

        return invoice;
    }
}