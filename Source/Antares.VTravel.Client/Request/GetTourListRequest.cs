namespace Antares.VTravel.Client.Request;

using Antares.VTravel.Client.Dto;
using Antares.VTravel.Shared; 
using MediatR;

public record GetTourListRequest(
    string FilterText,
    int Page = 1,
    int PerPage = 100)
    : IRequest<PaginatedList<TourDto>>;
