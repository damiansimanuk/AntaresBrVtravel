namespace Antares.VTravel.UI.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/*

dotnet ef migrations add InitialTour --startup-project ..\Antares.VTravel.UI\ -c ApplicationDbContext
*/

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{

}
