namespace Antares.VTravel.Shared.Dto;

public record TourDto(
    int Id,
    string Name,
    string Description,
    string UserId,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool Active = true);