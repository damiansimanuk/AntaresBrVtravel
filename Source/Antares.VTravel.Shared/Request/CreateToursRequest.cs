namespace Antares.VTravel.Shared.Request;

using Antares.VTravel.Shared.Core.Event;
using Antares.VTravel.Shared.Core.ResultFluent;
using Antares.VTravel.Shared.Dto;
using MediatR;

public record CreateTourRequest(
    bool Failure,
    bool Exception,
    string Name,
    string Description)
    : IRequest<Result<TourDto>>;

public record TourCreated(
    int TourId,
    string TourName,
    string Description
    ) : DomainEventBase<TourCreated>;
