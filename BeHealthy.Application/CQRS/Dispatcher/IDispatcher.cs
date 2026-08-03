using BeHealthy.Application.CQRS.Command;
using BeHealthy.Application.CQRS.Query;

namespace BeHealthy.Application.CQRS.Handler;

public interface IDispatcher
{
    Task<TResponse> SendQueryAsync<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>;

    Task<TResponse> SendCommandAsync<TCommand, TResponse>(
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>;
}
