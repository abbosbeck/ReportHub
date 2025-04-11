using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    public async Task<Customer> AddAsync(Customer customer)
    {
        await dbContext.AddAsync(customer);
        await dbContext.SaveChangesAsync();

        return customer;
    }
}