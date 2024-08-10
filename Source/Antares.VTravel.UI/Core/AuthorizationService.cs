namespace Antares.VTravel.UI.Core;
using Antares.VTravel.Shared.ResultFluent;

public class AuthorizationService(CurrentUser currentUser, IConfiguration configuration)
{
    public bool SkipAuthorization { get; set; } = configuration.GetValue("SkipAuthorization", false);

    private bool CheckClaimsPermission(string? permission, params string[] permissions)
    {
        if (SkipAuthorization)
        {
            return true;
        }

        if (permission is null && !permissions.Any())
        {
            return true;
        }

        var claims = currentUser.Principal.FindAll(c => c.Type == "permission" || c.Type == "permissions");

        return string.IsNullOrEmpty(permission)
            ? claims
                .Any(c => permissions.Any(v => c.Value.Contains(v)))
            : claims
                .Where(c => c.Value.Contains(permission))
                .Any(c => permissions.Any(v => c.Value.Contains(v)));
    }

    public Result<bool> IsValidatePermissions(params string[] permission)
    {
        return CheckClaimsPermission(null, permission)
            ? true
            : new Error("Unauthorize", "Insufficient permissions to perform this operation.");
    }

    public Result<bool> IsValidCustomPermissions(string value, params string[] permission)
    {
        return CheckClaimsPermission(value, permission)
            ? true
            : new Error("Unauthorize", "Insufficient permissions to perform this operation.");
    }
}