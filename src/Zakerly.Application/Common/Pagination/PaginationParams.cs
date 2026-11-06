namespace Zakerly.Application.Common.Pagination;

public sealed record PaginationParams(
    int PageNumber = 1,
    int PageSize = 20)
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    public int NormalizedPageNumber => PageNumber < 1 ? 1 : PageNumber;

    public int NormalizedPageSize
    {
        get
        {
            if (PageSize < 1) return DefaultPageSize;
            if (PageSize > MaxPageSize) return MaxPageSize;
            return PageSize;
        }
    }

    public long Skip => (long)(NormalizedPageNumber - 1) * NormalizedPageSize;
    public int Take => NormalizedPageSize;

    public PaginationMetadata CreateMetadata(long totalItems)
    {
        var pageNumber = NormalizedPageNumber;
        var pageSize = NormalizedPageSize;

        // Calculate total pages using overflow-safe integer arithmetic
        // totalPages = ceil(totalItems / pageSize)
        var totalPages = pageSize > 0
            ? totalItems / pageSize + (totalItems % pageSize == 0 ? 0 : 1)
            : 0;

        var hasPreviousPage = pageNumber > 1;
        var hasNextPage = pageNumber < totalPages;

        return new PaginationMetadata(
            pageNumber,
            pageSize,
            totalItems,
            totalPages,
            hasPreviousPage,
            hasNextPage);
    }
}