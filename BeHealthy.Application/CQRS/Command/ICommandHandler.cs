namespace BeHealthy.Application.CQRS.Command;

public interface ICommandHandler<in TCommand, out TResult>
    where TCommand : ICommand<TResult>
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
