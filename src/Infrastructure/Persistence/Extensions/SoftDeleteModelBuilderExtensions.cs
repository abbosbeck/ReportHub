using System.Linq.Expressions;
using Domain.Common;

namespace Infrastructure.Persistence.Extensions;

public static class SoftDeleteModelBuilderExtensions
{
    public static ModelBuilder ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var param = Expression.Parameter(entityType.ClrType, "entity");
            var prop = Expression.Property(param, nameof(ISoftDeletable.IsDeleted));
            var entityNotDeleted = Expression.Lambda(Expression.Equal(prop, Expression.Constant(false)), param);

            entityType.SetQueryFilter(entityNotDeleted);
        }

        return modelBuilder;
    }
}