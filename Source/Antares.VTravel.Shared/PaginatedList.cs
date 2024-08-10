namespace Antares.VTravel.Shared;

public class PaginatedList<TResult>
{
    public int Page { get; init; } = 1;
    public int PerPage { get; init; } = 1;
    public int Pages => 1 + RowsCount / PerPage;
    public int RowsCount { get; init; }
    public bool HasNextPage => Page < Pages;
    public bool HasPreviousPage => Page > 1;
    public List<TResult> Items { get; init; } = new();
}
