namespace Antares.VTravel.UI.Data;
using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static void EnsureDatabaseCreated<TContext>(this IHost host)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<TContext>()!;
        context.Database.Migrate();
    }
}
