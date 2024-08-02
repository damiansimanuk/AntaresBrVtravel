using Antares.VTravel.UI.Data;
using Microsoft.EntityFrameworkCore;

public static class ApplicationDbContextExtensions
{
    public static void EnsureDatabaseCreated(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<ApplicationDbContext>()!;
        context.Database.Migrate();
    }
}