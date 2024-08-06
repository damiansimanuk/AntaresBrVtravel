namespace Antares.VTravel.UI.Core.Database;
using Antares.VTravel.Shared.Core;
using Microsoft.EntityFrameworkCore;

public abstract class UnitOfWorkContext : DbContext
{

    public UnitOfWorkContext(DbContextOptions options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = EntitiesChanged();
        FillUpdatedAt();
        var result = await base.SaveChangesAsync(cancellationToken);
        PublishDomainEvents(entities);
        ChangeTracker.Clear();
        return result;
    }

    private List<IEntity> EntitiesChanged()
    {
        return ChangeTracker
            .Entries<IEntity>()
            .Where(entry => entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            .Select(e => e.Entity)
            .ToList();
    }

    public virtual void PublishDomainEvents(List<IEntity> entities)
    {
        if (entities.Any())
        {
            var events = entities
                .Select(e => e.GetDomainEvents())
                .Where(e => e != null)
                .SelectMany(e => e).ToArray();

            entities.ForEach(e => e.ClearDomainEvents());

            SendMessage(events);
        }
    }

    public virtual void SendMessage(params IDomainEvent[] events)
    {
    }

    private void FillUpdatedAt()
    {
        ChangeTracker.Entries<IEntity>()
            .Where(entry => entry.State == EntityState.Modified || entry.State == EntityState.Added)
            .Select(entry => entry.Member(nameof(IEntity.UpdatedAt)))
            .Where(member => member != null)
            .ToList()
            .ForEach(member => member.CurrentValue = DateTimeOffset.UtcNow);
    }
}