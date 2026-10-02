namespace BeHealthy.Tests.UnitTests.Fakes;

/// <summary>
/// Runs the operation directly and records whether the real transaction would have been committed or rolled back.
/// </summary>
public class FakeTransactionManager : ITransactionManager
{
    public bool Committed { get; private set; }
    public bool RolledBack { get; private set; }

    public async Task<ServiceResponse> ExecuteInTransactionAsync(
        Func<Task<ServiceResponse>> operation,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await operation();
            Committed = result.Success;
            RolledBack = !result.Success;
            return result;
        }
        catch
        {
            RolledBack = true;
            throw;
        }
    }
}
