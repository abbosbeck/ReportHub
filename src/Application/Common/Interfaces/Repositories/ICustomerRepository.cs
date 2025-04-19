using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<bool> DeleteAsync(Customer customer);

    Task<Customer> AddAsync(Customer customer);

    Task<Customer> UpdateAsync(Customer customer);

    Task<Customer> GetByIdAsync(Guid id);

    IQueryable<Customer> GetAll();
}