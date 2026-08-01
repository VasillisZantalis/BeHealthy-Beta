namespace BeHealthy.Application.CQRS.Query;

public interface IQueryHandler<in TQuery, out TResult>
    where TQuery : IQuery<TResult>
{
    Task HandleAsync(TQuery query, CancellationToken cancellationToken);
}
