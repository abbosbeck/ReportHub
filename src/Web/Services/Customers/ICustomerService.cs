namespace Web.Services.Customers;

public interface ICustomerService
{
    Task<List<CustomerResponse>> GetListAsync(Guid clientId);

    Task<CustomerResponse> GetByIdAsync(Guid id, Guid clientId);
}
