using System.Linq.Expressions;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<bool> DeleteAsync(Customer customer)
    {
        context.Remove(customer);

        return await context.SaveChangesAsync() > 0;
    }

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

    public IQueryable<Customer> GetAll()
    {
        return context.Customers;
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        return await context.Customers.FindAsync(id);
    }
}