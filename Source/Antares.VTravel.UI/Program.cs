using Antares.VTravel.Shared.Core;
using Antares.VTravel.Shared.Dto;
using Antares.VTravel.Shared.Request;
using Antares.VTravel.UI.Components;
using Antares.VTravel.UI.Components.Account;
using Antares.VTravel.UI.Data;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

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
        //builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApiDocument();

        builder.Services.AddAuthorization();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddIdentityCookies();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        builder.Services.AddHttpClient();
        //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddScoped<HttpMediator>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

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

        app.EnsureDatabaseCreated();

        app.Run();
    }
}
