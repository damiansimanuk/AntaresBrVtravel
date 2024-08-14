namespace Antares.VTravel.Shared;

public record struct PaginatedList<TResult>
{
    public List<TResult> Items { get; init; } = new();
    public int RowsCount { get; init; }
    public int RowsPerPage { get; init; } = 1;
    public int RowsOffset { get; init; } = 0;
    public int Page { get; init; } = 1;
    public int Pages { get; init; }
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }

    public PaginatedList(List<TResult> items, int rowsCount, int rowsPerPage, int? rowsOffset, int? page)
    {
        Items = items;
        RowsCount = rowsCount;
        RowsPerPage = rowsPerPage;
        Pages = 1 + ((rowsCount == 0 ? 0 : rowsCount - 1) / rowsPerPage);
        if (rowsOffset.HasValue)
        {
            RowsOffset = rowsOffset.Value;
            Page = page ?? ((rowsOffset.Value / rowsPerPage) + 1);
        }
        else if (page.HasValue)
        {
            Page = page.Value;
            RowsOffset = rowsOffset ?? ((page.Value - 1) * RowsPerPage);
        }
        HasNextPage = Page < Pages;
        HasPreviousPage = Page > 1;
    }
}
