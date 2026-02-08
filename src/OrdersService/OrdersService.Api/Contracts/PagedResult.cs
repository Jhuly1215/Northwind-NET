namespace OrdersService.Api.Contracts;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    long Total
)
{
    public long TotalPages => (long)Math.Ceiling(Total / (double)PageSize);
}
