namespace Antares.VTravel.UI.Core;
using System.Security.Claims;

public class CurrentUser(IHttpContextAccessor httpContextAccessor)
{
    public ClaimsPrincipal Principal { get; set; } = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    public string Name => Principal.Identity?.Name ?? "Anonymous";
    public string? Identifier => Principal.FindFirstValue(ClaimTypes.NameIdentifier);
    public bool IsAuthenticated => Principal.Identity?.IsAuthenticated ?? false;
}
