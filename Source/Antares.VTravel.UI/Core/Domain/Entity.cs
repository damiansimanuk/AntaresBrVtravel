namespace Antares.VTravel.UI.Core.Domain;
using System.Collections.Concurrent;
using Antares.VTravel.Shared.Core;
using Antares.VTravel.Shared.Core.Event;

public abstract class Entity<TKey> : IEntity<TKey>
{
    ConcurrentQueue<Func<IDomainEvent>> domainEvents = new();

    public TKey Id { get; protected set; } = default!;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return domainEvents.Select(e => e.Invoke()).ToList();
    }

    public void AddDomainEvent(Func<IDomainEvent> eventItem)
    {
        domainEvents.Enqueue(eventItem);
    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }
}