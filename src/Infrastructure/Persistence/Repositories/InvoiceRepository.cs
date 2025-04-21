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

    public IQueryable<Invoice> GetAll()
    {
        return context.Invoices.Include(invoice => invoice.Items);
    }

    public async Task<bool> DeleteAsync(Invoice invoice)
    {
        context.Remove(invoice);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Invoice> GetByIdAsync(Guid invoiceId)
    {
        return await context.Invoices
            .Include(i => i.Items)
            .Include(i => i.Customer)
            .Include(i => i.Client)
            .FirstOrDefaultAsync(i => i.Id == invoiceId);
    }

    public async Task<Invoice> UpdateAsync(Invoice invoice)
    {
        context.Update(invoice);
        await context.SaveChangesAsync();

        return invoice;
    }
}