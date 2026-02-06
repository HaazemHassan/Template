using Ardalis.Specification;
using System.Linq.Expressions;

namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;

public interface IGenericRepository<T> : IRepositoryBase<T> where T : class {

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate);
}
