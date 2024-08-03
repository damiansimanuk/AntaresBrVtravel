namespace Antares.VTravel.Shared.Core;

public record Error(
    string Code,
    string Message,
    string? Detail = null);

