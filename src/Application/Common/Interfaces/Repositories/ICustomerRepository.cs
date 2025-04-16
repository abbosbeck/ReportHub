using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
}