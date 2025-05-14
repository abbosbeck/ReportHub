using Web.Models.Customers;

namespace Web.Services.Customers;

public interface ICustomerService
{
    Task<List<CustomerResponse>> GetListAsync(Guid clientId);

    Task<CustomerResponse> GetByIdAsync(Guid id, Guid clientId);

    Task<bool> CreateAsync(CreateCustomerRequest customer, Guid clientId);

    Task<List<CustomerResponse>> UploadCustomerListAsync(byte[] file, string name, Guid clientId);

    Task<bool> DeleteAsync(Guid id, Guid clientId);
}
