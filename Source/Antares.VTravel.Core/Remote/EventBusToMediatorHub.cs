namespace Antares.VTravel.Core.Remote;
using Microsoft.AspNetCore.SignalR;
using Antares.VTravel.Shared.Event;
using Antares.VTravel.Shared.Remote;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;

public class EventBusToMediatorHub(IHubContext<MediatorHubServer> hubContext, DomainEventBus events) : IHostedService
{
    private IDisposable? disposable;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        disposable = events.SubscribeAll(OnNextMessage);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        disposable?.Dispose();
        return Task.CompletedTask;
    }

    private void OnNextMessage(IDomainEvent e)
    {
        try
        {
            hubContext.Clients.Group(e.EventName).SendAsync("OnNextMessage", HubRequestSerializer.Wrap(e));
        }
        catch { }
    }
}
