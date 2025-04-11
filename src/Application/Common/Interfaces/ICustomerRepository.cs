using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);

    Task<Customer> GetCustomerById(Guid id);

    Task<Customer> GetCustomerByEmail(string email);

    bool CheckIsCustomerExistByEmail(string email);

    Task<IEnumerable<Customer>> GetAllCustomer();

    Task UpdateCustomerAsync(Guid id);

    Task SaveChangesAsync();
}
