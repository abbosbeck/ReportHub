using Quartz;

namespace Application.Reports.ScheduleReport;

public class ScheduleReportRequestValidator : AbstractValidator<ScheduleReportRequest>
{
    public ScheduleReportRequestValidator()
    {
        RuleFor(request => request.CronExpression)
            .Must(CronExpression.IsValidExpression)
            .WithMessage("Invalid Cron Expression!");
    }
}