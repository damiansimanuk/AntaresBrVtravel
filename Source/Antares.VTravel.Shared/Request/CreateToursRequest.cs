namespace Antares.VTravel.Shared.Request;

using Antares.VTravel.Shared.Dto;
using MediatR;

public record CreateToursRequest(
    string Name,
    string Description)
    : IRequest<TourDto>;
