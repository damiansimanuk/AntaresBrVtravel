namespace Antares.VTravel.UI.Data;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public virtual string? TestX { get; set; }
}
