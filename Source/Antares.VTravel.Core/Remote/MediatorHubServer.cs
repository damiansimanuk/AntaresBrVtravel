namespace Antares.VTravel.Core.Remote;
using Antares.VTravel.Shared.Core.Event;
using MediatR;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using System;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Logging;
using Antares.VTravel.Shared.Request;
using System.Text.Json.Nodes;
using System.Text.Json;
using Antares.VTravel.Shared.Core.Remote;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using Antares.VTravel.Shared.Core.ResultFluent;

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

public class MediatorHubServer(
    EventBusToMediatorHub _,
    IMediator mediator,
    ILogger<MediatorHubServer> logger
    ) : Hub
{
    HashSet<string> subscriptions = new();
    List<IDisposable> disposables = new();

    public async Task<object> Request(JsonElement request)
    {
        try
        {
            var content = HubRequestSerializer.Deserialize(request);
            var res = await mediator.Send(content);
            return HubRequestSerializer.Wrap(res!);
        }
        catch (Exception ex)
        {
            ex = ex.InnerException ?? ex;
            var code = ex.GetType().Name;
            var error = new Error(code, ex.Message, ex.StackTrace);
            return HubRequestSerializer.Wrap(error);
        }
    }

    public async Task Subscribe(string eventName)
    {
        logger.LogInformation("Subscribe {0}", eventName);
        await Groups.AddToGroupAsync(Context.ConnectionId, eventName);
    }

    public async Task Unsubscribe(string eventName)
    {
        logger.LogInformation("Unsubscribe {0}", eventName);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventName);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        logger.LogInformation("OnDisconnectedAsync");
        return base.OnDisconnectedAsync(exception);
    }

    public override Task OnConnectedAsync()
    {
        logger.LogInformation("OnConnectedAsync");
        return base.OnConnectedAsync();
    }
}
