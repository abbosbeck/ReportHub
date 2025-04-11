using System.Runtime.Serialization;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task AddCustomerAsync(Customer customer)
    {
        await context.Customers.AddAsync(customer);
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<Customer>> GetAllCustomer()
    {
        return await context.Customers.ToListAsync();
    }

    public Task<Customer> GetCustomerById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool CheckIsCustomerExistByEmail(string email)
{
        return context.Customers.Any(c => c.Email == email);
    }

    public async Task<Customer> GetCustomerByEmail(string email)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(c => c.Email == email);

        return customer;
    }

    public Task UpdateCustomerAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}