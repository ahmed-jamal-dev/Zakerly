namespace Zakerly.Application.Common.Pagination;

public sealed record PaginationMetadata(
    int PageNumber,
    int PageSize,
    long TotalItems,
    long TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);