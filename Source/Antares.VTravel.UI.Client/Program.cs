using Antares.VTravel.Shared.Remote; 
using Antares.VTravel.UI.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
builder.Services.AddSingleton(sp => new MediatorHubClient(new Uri(baseAddress, "signalr-mediator")));
builder.Services.AddScoped<HttpMediator>();

await builder.Build().RunAsync();
