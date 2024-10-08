namespace Antares.VTravel.UI.Handlers;
using Antares.VTravel.Client.Dto;
using Antares.VTravel.Client.Request;
using Antares.VTravel.UI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Antares.VTravel.UI.Mapper;
using Antares.VTravel.Shared;

public class GetTourListRequestHandler(
    VTravelDbContext dbContext,
    MapperService mapper
    ) : IRequestHandler<GetTourListRequest, PaginatedList<TourDto>>
{
    public async Task<PaginatedList<TourDto>> Handle(GetTourListRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Tour>()
            .Where(t => t.Name.Contains(request.FilterText) || t.Description.Contains(request.FilterText));

        var rowsCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(request.RowsOffset)
            .Take(request.RowsPerPage)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TourDto>(
            items.Select(mapper.ToDto).ToList(),
            rowsCount,
            request.RowsPerPage,
            request.RowsOffset,
            null
        );
    }
}