namespace Antares.VTravel.Shared.Request;

using Antares.VTravel.Shared.Core;
using Antares.VTravel.Shared.Dto;
using MediatR;

public record GetTourListRequest(
    string FilterText,
    int Page = 1,
    int PerPage = 100)
    : IRequest<PaginatedList<TourDto>>;
