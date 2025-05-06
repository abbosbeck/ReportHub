using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces.Services;

public interface IImportDataFromFileService
{
    List<Customer> ImportCustomerListFromExcel(IFormFile file);
}
