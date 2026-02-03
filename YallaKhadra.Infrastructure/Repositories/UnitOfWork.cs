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

    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken) {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new DataBaseTransaction(_transaction);
    }

}
