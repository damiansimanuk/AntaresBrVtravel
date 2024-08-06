namespace Antares.VTravel.Shared.Core;

public record Error(
    string Code,
    string Message,
    string? Detail = null)
{
    public static Error Create(string code, string message, string detail = null!) => new Error(code, message, detail);
    public static Error Invalid(string message, string detail = null!) => new Error("Invalid", message, detail);
}

