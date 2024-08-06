namespace Antares.VTravel.UI.Core;
using Antares.VTravel.Shared.Core;

public class AuthorizationService(CurrentUser currentUser, IConfiguration configuration)
{  
    public bool _skipAuthorization { get; set; }= configuration.GetValue("SkipAuthorization", false);
  
    private bool CheckClaimsPermission(string? permission, params string[] permissions)
    {
        if (_skipAuthorization)
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
        var allowed = CheckClaimsPermission(null, permission);
        return ErrorBuilder.Create()
            .Add(!allowed, () => new Error("Unauthorize", "Insufficient permissions to perform this operation."))
            .ToResult(allowed);
    }

    public Result<bool> IsValidCustomPermissions(string value, params string[] permission)
    {
        var allowed = CheckClaimsPermission(value, permission);
        return ErrorBuilder.Create()
            .Add(!allowed, () => new Error("Unauthorize", "Insufficient permissions to perform this operation."))
            .ToResult(allowed);
    }
}