using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Infrastructure.Data;

namespace YallaKhadra.Infrastructure.Repositories {
    public class GenericRepository<T> : IGenericRepository<T> where T : class {


        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext dbContext) {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }


        public virtual async Task<T?> GetByIdAsync(int id) {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) {
            return await _dbContext.Set<T>().AnyAsync(predicate);
        }

        public virtual async Task<T> AddAsync(T entity) {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public virtual async Task AddRangeAsync(ICollection<T> entities) {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }
        public virtual async Task UpdateAsync(T entity) {
            _dbContext.Set<T>().Update(entity);
        }
        public virtual async Task UpdateRangeAsync(ICollection<T> entities) {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity) {
            _dbContext.Set<T>().Remove(entity);
        }
        public virtual async Task DeleteRangeAsync(ICollection<T> entities) {
            foreach (var entity in entities) {
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
        }


        public IQueryable<T> GetTableNoTracking() {
            return _dbSet.AsNoTracking().AsQueryable();
        }


        public async Task<PaginatedResult<TResult>> GetPaginatedListAsync<TResult>(IQueryable<TResult> source, int pageNumber, int pageSize) where TResult : class {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int count = await source.CountAsync();
            if (count == 0)
                return PaginatedResult<TResult>.Success(new List<TResult>(), count, pageNumber, pageSize);

            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return PaginatedResult<TResult>.Success(items, count, pageNumber, pageSize);
        }
    }
}
