using System.Linq.Expressions;

namespace BeHealthy.Application.Common.Models;

public class QueryOptions<T>
{
    public Expression<Func<T, bool>>? Predicate { get; set; }
    public bool TrackChanges { get; set; } = false;
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; set; } = new();
    public Expression<Func<T, object>>? OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;
}
