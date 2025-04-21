using Domain.Entities;

namespace Application.Invoices.GetExportLogById;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogDto>();
    }
}