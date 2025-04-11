using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
}