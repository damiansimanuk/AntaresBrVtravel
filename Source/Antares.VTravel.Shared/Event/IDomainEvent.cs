namespace Antares.VTravel.Shared.Event;

using Antares.VTravel.Shared.ResultFluent;
using MediatR;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

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
