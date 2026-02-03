using System.Linq.Expressions;
using YallaKhadra.Core.Bases.Responses;

namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts;

public interface IGenericRepository<T> where T : class {
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(ICollection<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(ICollection<T> entities);
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetTableNoTracking();
    Task<PaginatedResult<TResult>> GetPaginatedListAsync<TResult>(IQueryable<TResult> source, int pageNumber, int pageSize) where TResult : class;

}
