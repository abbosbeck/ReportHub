using Quartz;

namespace Application.ReportSchedules.ScheduleReport;

public class ScheduleReportRequestValidator : AbstractValidator<ScheduleReportRequest>
{
    public ScheduleReportRequestValidator()
    {
        RuleFor(request => request.CronExpression)
            .Must(CronExpression.IsValidExpression)
            .WithMessage("Invalid Cron Expression!");
    }
}