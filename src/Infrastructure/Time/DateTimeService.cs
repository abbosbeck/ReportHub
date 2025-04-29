using Application.Common.Interfaces.Time;

namespace Infrastructure.Time;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}