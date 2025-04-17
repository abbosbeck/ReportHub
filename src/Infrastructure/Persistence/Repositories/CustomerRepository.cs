using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<Customer> AddAsync(Customer customer)
    {
        await context.AddAsync(customer);
        await context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        context.Update(customer);
        await context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        return await context.Customers.FindAsync(id);
    }
}