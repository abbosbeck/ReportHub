using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.HasMany(p => p.PlanItems)
            .WithOne(pi => pi.Plan)
            .HasForeignKey(pi => pi.PlanId);
    }
}
