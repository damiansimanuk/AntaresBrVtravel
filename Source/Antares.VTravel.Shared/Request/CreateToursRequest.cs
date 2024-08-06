namespace Antares.VTravel.Shared.Request;

using Antares.VTravel.Shared.Core;
using Antares.VTravel.Shared.Dto;
using MediatR;

public record CreateTourRequest(
    bool failure,
    string Name,
    string Description)
    : IRequest<Result<TourDto>>;
