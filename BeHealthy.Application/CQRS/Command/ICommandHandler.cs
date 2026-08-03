namespace BeHealthy.Application.CQRS.Command;

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(
       TCommand command,
       CancellationToken cancellationToken);
}
