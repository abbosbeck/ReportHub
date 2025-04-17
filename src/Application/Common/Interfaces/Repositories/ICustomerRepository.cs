using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);

    Task<Customer> UpdateAsync(Customer customer);

    Task<Customer> GetAsync(Expression<Func<Customer, bool>> expression);

    IQueryable<Customer> GetAll(Expression<Func<Customer, bool>> expression = null);
}