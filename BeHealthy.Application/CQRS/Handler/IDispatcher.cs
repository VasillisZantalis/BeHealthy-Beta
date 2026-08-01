using BeHealthy.Application.CQRS.Command;
using BeHealthy.Application.CQRS.Query;

namespace BeHealthy.Application.CQRS.Handler;

public interface IDispatcher
{
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
}
