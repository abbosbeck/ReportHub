using System.Linq.Expressions;
using Domain.Common;

namespace Infrastructure.Persistence.Extensions;

public static class SoftDeleteModelBuilderExtensions
{
    public static ModelBuilder ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(entityType => typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            .ToList();

        foreach (var entityType in entityTypes)
        {
            var param = Expression.Parameter(entityType.ClrType, "entity");
            var prop = Expression.Property(param, nameof(ISoftDeletable.IsDeleted));
            var entityNotDeleted = Expression.Lambda(Expression.Equal(prop, Expression.Constant(false)), param);

            entityType.SetQueryFilter(entityNotDeleted);
        }

        return modelBuilder;
    }
}