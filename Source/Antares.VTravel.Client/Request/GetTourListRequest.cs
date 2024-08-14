namespace Antares.VTravel.Client.Request;

using Antares.VTravel.Client.Dto;
using Antares.VTravel.Shared;
using Antares.VTravel.Shared.ResultFluent;
using MediatR;

public record GetTourListRequest(
    string FilterText,
    int RowsOffset = 1,
    int RowsPerPage = 100)
    : IRequest<PaginatedList<TourDto>>;
