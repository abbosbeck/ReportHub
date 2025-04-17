using Application.Customers.UpdateCustomer;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);

    Task<Customer> UpdateAsync(Customer customer);

    Task<Customer> GetByIdAsync(Guid id);
}