namespace Antares.VTravel.Shared.Dto;

public record struct TourDto(
    int Id,
    string Name,
    string Description,
    string UserId,
    DateTimeOffset UpdatedAt,
    DateTimeOffset CreatedAt,
    bool Active);