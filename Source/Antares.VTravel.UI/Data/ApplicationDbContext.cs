namespace Antares.VTravel.UI.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/*
dotnet ef migrations add InitialApp --verbose -c ApplicationDbContext -o Migrations/ApplicationDb  -- --provider SqlServer
dotnet ef migrations add InitialTour --verbose -c VTravelDbContext -o Migrations/VTravelDb -- --provider SqlServer
*/

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}
