using System.Linq.Expressions;
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

    public async Task<Customer> GetAsync(Expression<Func<Customer, bool>> expression)
    {
        return await context.Customers.FirstOrDefaultAsync(expression);
    }

    public IQueryable<Customer> GetAll(Expression<Func<Customer, bool>> expression = null)
    {
        return expression is null ? context.Customers : context.Customers.Where(expression);
    }
}