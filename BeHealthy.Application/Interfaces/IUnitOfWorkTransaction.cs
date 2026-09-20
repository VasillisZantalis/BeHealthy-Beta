namespace BeHealthy.Application.Interfaces;

public interface IUnitOfWorkTransaction : IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}
