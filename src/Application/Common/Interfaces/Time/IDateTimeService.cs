namespace Application.Common.Interfaces.Time;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}