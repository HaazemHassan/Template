using Microsoft.EntityFrameworkCore.Storage;

namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts;

public interface IUnitOfWork : IAsyncDisposable {

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}
