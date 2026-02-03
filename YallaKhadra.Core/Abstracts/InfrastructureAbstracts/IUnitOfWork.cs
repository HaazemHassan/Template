namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts;

public interface IUnitOfWork {

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

}
