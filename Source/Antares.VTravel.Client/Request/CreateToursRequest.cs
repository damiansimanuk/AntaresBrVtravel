namespace Antares.VTravel.Client.Request;

using Antares.VTravel.Client.Dto;
using MediatR;
using Antares.VTravel.Shared.ResultFluent;

public record CreateTourRequest(
    bool Failure,
    bool Exception,
    string Name,
    string Description)
    : IRequest<Result<TourDto>>;
