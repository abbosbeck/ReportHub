using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<bool> DeleteAsync(Customer customer);

    Task<Customer> AddAsync(Customer customer);

    Task AddBulkAsync(List<Customer> customers);

    Task<Customer> UpdateAsync(Customer customer);

    Task<Customer> GetByIdAsync(Guid id);

    IQueryable<Customer> GetAll();
}