using BeHealthy.Application.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BeHealthy.Infrastructure.Data;

public class EfTransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _context;

    public EfTransactionManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse> ExecuteInTransactionAsync(
        Func<Task<ServiceResponse>> operation,
        CancellationToken cancellationToken = default)
    {
        // Already inside a transaction: let the outermost call decide commit/rollback.
        if (_context.Database.CurrentTransaction is not null)
            return await operation();

        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async ct =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                var result = await operation();

                if (result.Success)
                {
                    await transaction.CommitAsync(ct);
                }
                else
                {
                    await RollbackAsync(transaction);
                }

                return result;
            }
            catch
            {
                await RollbackAsync(transaction);
                throw;
            }
        }, cancellationToken);
    }

    private async Task RollbackAsync(IDbContextTransaction transaction)
    {
        // Never cancel a rollback: it must run even when the request was aborted.
        await transaction.RollbackAsync(CancellationToken.None);
        _context.ChangeTracker.Clear();
    }
}
