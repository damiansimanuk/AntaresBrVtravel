namespace Antares.VTravel.Client.Event;
using Antares.VTravel.Shared.Event;

public record TourCreated(
    int TourId,
    string TourName,
    string Description
    ) : DomainEventBase<TourCreated>;
