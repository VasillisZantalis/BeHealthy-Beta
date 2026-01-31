namespace BeHealthy.Shared.Parameters;

public class QueryParameters
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;
}
