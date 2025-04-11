using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class InvoiceRepository(AppDbContext context) : IInvoiceRepository
    {
        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            ArgumentNullException.ThrowIfNull(invoice);

            invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                ClientId = invoice.ClientId,
                CustomerId = invoice.CustomerId,
                PaymentStatus = invoice.PaymentStatus,
                Items = invoice.Items?.Select(item => new Item
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    PaymentStatus = item.PaymentStatus,
                    Currency = item.Currency,
                    ClientId = item.ClientId,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                }).ToList(),
            };

            await context.Invoices.AddAsync(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> DeleteAsync(Guid invoiceId)
        {
            var invoice = await context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
            {
                return false;
            }

            invoice.IsDeleted = true;
            invoice.DeletedOn = DateTime.UtcNow;

            if (invoice.Items != null)
            {
                foreach (var item in invoice.Items)
                {
                    item.IsDeleted = true;
                    item.DeletedOn = DateTime.UtcNow;
                }
            }

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await context.Invoices
                 .Include(i => i.Client)
                 .Include(i => i.Customer)
                 .Include(i => i.Items)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetByClientIdAsync(Guid clientId)
        {
            return await context.Invoices
                .Include(i => i.Client)
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .Where(i => i.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Invoice> GetByIdAsync(Guid invoiceId)
        {
            return await context.Invoices
                .Include(i => i.Client)
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);
        }
    }
}
