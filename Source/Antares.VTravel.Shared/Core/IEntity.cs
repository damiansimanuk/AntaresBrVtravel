namespace Antares.VTravel.Shared.Core;

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
