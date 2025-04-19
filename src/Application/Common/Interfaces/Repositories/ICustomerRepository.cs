using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<bool> DeleteAsync(Customer customer);

    Task<Customer> AddAsync(Customer customer);

    Task<Customer> UpdateAsync(Customer customer);

    Task<Customer> GetAsync(Expression<Func<Customer, bool>> expression);

    Task<Customer> GetByIdAsync(Guid id);

    IQueryable<Customer> GetAll(Expression<Func<Customer, bool>> expression = null);
}