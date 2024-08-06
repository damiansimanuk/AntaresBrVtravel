namespace Antares.VTravel.UI.Core;
using System.Security.Claims;
using Antares.VTravel.UI.Data;

public class CurrentUser(IHttpContextAccessor httpContextAccessor)
{  
    public ClaimsPrincipal Principal { get; set; } = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    public string Name => Principal.Identity?.Name ?? "Anonymous";
    public bool IsAuthenticated => Principal.Identity?.IsAuthenticated ?? false;
}
