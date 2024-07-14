namespace Post.Host.Models.Response;

public class PaginatedResponse<T>
{
    public string? SearchByUserName { get; set; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public string? SearchByTitle { get; init; }
    public long Count { get; init; }
    public IEnumerable<T> Data { get; init; } = null!;
}
