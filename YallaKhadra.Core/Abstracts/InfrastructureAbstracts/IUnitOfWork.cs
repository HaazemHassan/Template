
namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts;

public interface IUnitOfWork {

    // Repository Properties
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

}
