namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts {
    public interface IDatabaseTransaction : IAsyncDisposable {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);

    }
}