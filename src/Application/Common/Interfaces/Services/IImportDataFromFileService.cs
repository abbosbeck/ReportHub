using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces.Services;

public interface IImportDataFromFileService
{
    List<Customer> ImportCustomersDataFromExcel(IFormFile file);
}
