using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Time;
using Domain.Common;

namespace Infrastructure.Persistence.Interceptors;

public class SoftDeletableInterceptor(ICurrentUserService currentUserService, IDateTimeService dateTimeService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        DeleteEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DeleteEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void DeleteEntities(DbContext context)
    {
        if (context == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State != EntityState.Deleted)
            {
                continue;
            }

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedOn = dateTimeService.UtcNow;
            entry.Entity.DeletedBy = currentUserService.UserId.ToString();
        }
    }
}
