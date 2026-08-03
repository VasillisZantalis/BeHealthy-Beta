using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BeHealthy.Application.CQRS.Dispatcher;

public class Dispatcher(
    IServiceProvider serviceProvider,
    IValidatorService validatorService,
    ILogger<Dispatcher> logger) : IDispatcher
{
    public async Task<TResponse> SendCommandAsync<TCommand, TResponse>(
        TCommand command, 
        CancellationToken cancellationToken = default) 
        where TCommand : ICommand<TResponse>
    {
        ArgumentNullException.ThrowIfNull(command);

        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();

        using var scope = logger.BeginScope(
           new Dictionary<string, object>
           {
               ["CommandType"] = typeof(TCommand).Name
           });

        try
        {
            logger.LogInformation("Executing command");

            await validatorService.ValidateAsync(command, cancellationToken);
            return await handler.HandleAsync(command, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while executing command {CommandType}", typeof(TCommand).Name);
            throw;
        }
    }

    public async Task<TResponse> SendQueryAsync<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
    {
        ArgumentNullException.ThrowIfNull(query);

        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();

        using var scope = logger.BeginScope(
          new Dictionary<string, object>
          {
              ["CommandType"] = typeof(TQuery).Name
          });

        try
        {
            logger.LogInformation("Executing query");

            await validatorService.ValidateAsync(query, cancellationToken);
            return await handler.HandleAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while executing query {QueryType}", typeof(TQuery).Name);
            throw;
        }
    }
}
