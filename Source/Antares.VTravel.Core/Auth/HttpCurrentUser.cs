namespace Antares.VTravel.Core.Auth;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class HttpCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public ClaimsPrincipal Principal { get; set; } = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    public string Name => Principal.Identity?.Name ?? "Anonymous";
    public string? Identifier => Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public bool IsAuthenticated => Principal.Identity?.IsAuthenticated ?? false;
}
