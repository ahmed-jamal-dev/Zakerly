namespace Zakerly.Application.Common.Pagination;

public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items,
    PaginationMetadata Pagination);