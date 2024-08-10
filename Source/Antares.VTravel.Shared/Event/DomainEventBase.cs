namespace Antares.VTravel.Shared.Event;

using System;

public record DomainEventBase<T> : IDomainEvent<T>
{
    private static string guid = Guid.NewGuid().ToString("D");

    private static int count = 1;
    public string EventName { get; set; }
    public string EventId { get; }
    public bool IsIntegration { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    protected DomainEventBase()
    {
        EventId ??= $"{guid}-{++count}";
        EventName ??= GetType().Name;
    }
}
