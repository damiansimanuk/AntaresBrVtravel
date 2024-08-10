namespace Antares.VTravel.Shared.Core.Event;

using MediatR;

public interface IDomainEvent : INotification
{
    public string EventId { get; }
    public string EventName { get; }
    public bool IsIntegration { get; }
    public DateTimeOffset CreatedAt { get; set; }
}

public interface IDomainEvent<in T> : IDomainEvent
{
}
