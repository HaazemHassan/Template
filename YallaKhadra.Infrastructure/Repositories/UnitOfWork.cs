using Microsoft.EntityFrameworkCore.Storage;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Infrastructure.Data;

namespace YallaKhadra.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork {
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;


    public UnitOfWork(AppDbContext context) {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return _transaction;
    }

    public async Task CommitAsync(CancellationToken cancellationToken) {
        if (_transaction != null)
            await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken) {
        if (_transaction != null)
            await _transaction.RollbackAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync() {
        if (_transaction != null)
            await _transaction.DisposeAsync();

        await _context.DisposeAsync();
    }
}
