using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.HasQueryFilter(i => !i.IsDeleted);

        builder.HasMany(p => p.Items)
            .WithMany(i => i.Plans)
            .UsingEntity<PlanItem>();

        builder.Ignore(i => i.TotalPrice);
    }
}
