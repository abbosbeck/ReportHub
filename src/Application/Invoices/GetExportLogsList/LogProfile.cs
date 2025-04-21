using Domain.Entities;

namespace Application.Invoices.GetExportLogsList;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogDto>();
    }
}