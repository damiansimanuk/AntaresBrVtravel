using Antares.VTravel.Core.Remote;
using Antares.VTravel.Client.Dto;
using Antares.VTravel.Client.Event;
using Antares.VTravel.Shared.Remote;
using Antares.VTravel.Client.Request;
using Antares.VTravel.UI;
using Antares.VTravel.UI.Components;
using Antares.VTravel.UI.Components.Account;
using Antares.VTravel.UI.Core;
using Antares.VTravel.UI.Data;
using Antares.VTravel.UI.Mapper;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Antares.VTravel.Shared.Event;

internal class Program
{
    public static Assembly? ClientAssembly { get; private set; }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddRadzenComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument();

        builder.Services.AddAuthorization();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<VTravelDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<DomainEventBus>();
        builder.Services.AddSingleton<EventBusToMediatorHub>();
        //builder.Services.AddScoped<MediatorHubClient>(s => new MediatorHubClient(new Uri(" ")));
        //builder.Services.AddScoped<HttpMediator>();
        builder.Services.AddScoped<CurrentUser>();
        builder.Services.AddScoped<AuthorizationService>();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<MapperService>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        builder.Services.AddSignalR();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseOpenApi();
            app.UseSwaggerUi();
            app.UseWebAssemblyDebugging();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
        }

        app.MapGroup("/auth").MapIdentityApi<ApplicationUser>();

        app.MapPost(HttpMediator.EndpointName, (IMediator m, MediatorPostValueDto r) => m.Send(HttpMediator.Deserialize(r)!));
        //app.MapBlazorHub();
        app.MapHub<MediatorHubServer>("/signalr-mediator");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGroup("/api").MapControllers();

        app.UseStaticFiles();
        app.UseAntiforgery();

        var razorComponents = app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode();

        try
        {
            var exeFileName = Process.GetCurrentProcess().MainModule!.FileName;
            var exeName = Path.GetFileName(exeFileName);
            var exeDirectory = Path.GetDirectoryName(exeFileName)!;
            var clientName = exeName.Replace(".exe", ".Client.dll");
            var clientDll = Path.Combine(exeDirectory, clientName);
            ClientAssembly = Assembly.LoadFile(clientDll);
            razorComponents.AddAdditionalAssemblies(ClientAssembly);
        }
        catch { }

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.EnsureDatabaseCreated<ApplicationDbContext>();
        app.EnsureDatabaseCreated<VTravelDbContext>();

        app.Run();
    }
}
