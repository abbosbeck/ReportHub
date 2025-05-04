using Quartz;

namespace Application.ReportSchedules.ReScheduleReport;

public class ReScheduleReportRequestValidator : AbstractValidator<ReScheduleReportRequest>
{
    public ReScheduleReportRequestValidator()
    {
        RuleFor(request => request.CronExpression)
            .Must(CronExpression.IsValidExpression)
            .WithMessage("Invalid Cron Expression!");
    }
}