using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReportScheduleConfiguration : IEntityTypeConfiguration<ReportSchedule>
{
    public void Configure(EntityTypeBuilder<ReportSchedule> builder)
    {
        builder.Property(reportSchedule => reportSchedule.CronExpression)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(reportSchedule => reportSchedule.JobKey)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(reportSchedule => reportSchedule.TriggerKey)
            .HasMaxLength(200)
            .IsRequired();
    }
}