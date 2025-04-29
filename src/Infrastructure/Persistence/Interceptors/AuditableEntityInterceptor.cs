using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Time;
using Domain.Common;

namespace Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor(ICurrentUserService currentUserService, IDateTimeService dateTimeService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext context)
    {
        if (context == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = dateTimeService.UtcNow;
                entry.Entity.CreatedBy = currentUserService.UserId.ToString();
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedOn = dateTimeService.UtcNow;
                entry.Entity.LastModifiedBy = currentUserService.UserId.ToString();
            }
        }
    }
}
