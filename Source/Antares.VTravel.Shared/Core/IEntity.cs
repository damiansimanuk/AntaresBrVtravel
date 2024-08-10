namespace Antares.VTravel.Shared.Core;
using Antares.VTravel.Shared.Core.Event;

public interface IEntity
{
    DateTimeOffset? UpdatedAt { get; set; }
    DateTimeOffset? CreatedAt { get; set; }
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}
