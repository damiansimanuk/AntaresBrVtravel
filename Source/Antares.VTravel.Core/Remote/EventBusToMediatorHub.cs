namespace Antares.VTravel.Core.Remote;
using Microsoft.AspNetCore.SignalR;
using Antares.VTravel.Shared.Event;
using Antares.VTravel.Shared.Remote;

public class EventBusToMediatorHub
{
    private readonly IHubContext<MediatorHubServer> hubContext;
    private readonly DomainEventBus events;

    public EventBusToMediatorHub(IHubContext<MediatorHubServer> hubContext, DomainEventBus events)
    {
        this.hubContext = hubContext;
        this.events = events;
        this.events.SubscribeAll(OnNextMessage);
    }

    private void OnNextMessage(IDomainEvent e)
    {
        hubContext.Clients.Group(e.EventName).SendAsync("OnNextMessage", HubRequestSerializer.Wrap(e));
    }
}
