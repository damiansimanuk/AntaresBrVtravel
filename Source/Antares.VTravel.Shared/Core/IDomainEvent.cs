namespace Antares.VTravel.Shared.Core;

using MediatR;

public interface IDomainEvent : INotification
{
    public string EventId { get; }
    public string Name { get; }
    public bool IsIntegration { get; }
    public DateTime CreatedAt { get; set; }
}
