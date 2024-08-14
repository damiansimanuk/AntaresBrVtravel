﻿namespace Antares.VTravel.Shared.Remote;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;
using Antares.VTravel.Shared.ResultFluent;
using Antares.VTravel.Shared.Event;

public class MediatorHubClient
{
    private DomainEventBus events = new();
    private HubConnection hub;
    private bool connected;

    public MediatorHubClient(Uri url)
    {
        hub = new HubConnectionBuilder().WithUrl(url).Build();
        events.SubscriptionAdded += OnSubscriptionAdded;
        events.SubscriptionRemoved += OnSubscriptionRemoved;
        hub.On<JsonElement>("OnNextMessage", OnNextMessage);
        hub.Reconnected += OnReconnected;
    }

    public async Task StartAsync()
    {
        if (!connected)
        {
            await hub.StartAsync();
            await OnReconnected(null!);
        }
    }

    public async Task<Result<TResponse>> RequestAsync<TResponse>(IRequest<Result<TResponse>> comando)
    {
        var jsonResponse = await hub.InvokeAsync<JsonElement>("Request", HubRequestSerializer.Wrap(comando));
        var response = HubRequestSerializer.Deserialize(jsonResponse);
        return response is Error err
            ? Result.Failure<TResponse>(err)
            : (Result<TResponse>)response;
    }

    public async Task<Result<TResponse>> RequestAsync<TResponse>(IRequest<TResponse> comando)
    {
        var jsonResponse = await hub.InvokeAsync<JsonElement>("Request", HubRequestSerializer.Wrap(comando));
        var response = HubRequestSerializer.Deserialize(jsonResponse);
        return response is Error err
            ? Result.Failure<TResponse>(err)
            : Result.Success((TResponse)response);
    }

    public IDisposable Subscribe<TEvent>(Action<TEvent> onNextMessage) where TEvent : IDomainEvent
    {
        return events.Subscribe(onNextMessage);
    }

    public IDisposable SubscribeAll(Action<IDomainEvent> onNextMessage)
    {
        return events.SubscribeAll(onNextMessage);
    }

    private void OnNextMessage(JsonElement notification)
    {
        Console.WriteLine($"MediatorHubClient OnNextMessage {notification}");
        var msg = HubRequestSerializer.Deserialize(notification) as IDomainEvent;
        events.SendMessage(msg!);
    }

    private void OnSubscriptionAdded(object? sender, string eventName)
    {
        try
        {
            Console.WriteLine($"MediatorHubClient OnSubscriptionAdded {eventName}");
            hub.InvokeAsync("Subscribe", eventName);
        }
        catch { }
    }

    private void OnSubscriptionRemoved(object? sender, string eventName)
    {
        try
        {
            Console.WriteLine($"MediatorHubClient OnSubscriptionRemoved {eventName}");
            hub.InvokeAsync("Unsubscribe", eventName);
        }
        catch { }
    }

    private async Task OnReconnected(string? arg)
    {
        Console.WriteLine($"MediatorHubClient OnReconnected ");
        connected = true;
        foreach (var eventName in events.GetEventNames())
        {
            try
            {
                Console.WriteLine("OnReconnected Subscribe {0}", eventName);
                await hub.InvokeAsync("Subscribe", eventName);
            }
            catch { }
        }
    }
}
