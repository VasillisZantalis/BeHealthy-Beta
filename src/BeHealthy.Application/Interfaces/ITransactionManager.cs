namespace BeHealthy.Application.Interfaces;

/// <summary>
/// Runs an operation inside a single database transaction.
/// A single <c>SaveChangesAsync()</c> is already atomic, so this is only needed when an operation
/// persists more than once - e.g. ASP.NET Identity's UserManager (which saves on its own) plus our entities.
/// The transaction is committed only when the operation returns a successful response;
/// a failed response or an exception rolls it back and discards the staged changes.
/// </summary>
public interface ITransactionManager
{
    Task<ServiceResponse> ExecuteInTransactionAsync(
        Func<Task<ServiceResponse>> operation,
        CancellationToken cancellationToken = default);
}
