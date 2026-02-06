using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Infrastructure.Data;

namespace YallaKhadra.Infrastructure.Repositories {
    public class GenericRepository<T> : RepositoryBase<T>, IGenericRepository<T> where T : class {


        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) {
            return await _dbContext.Set<T>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate) {
            if (predicate is not null)
                return await _dbContext.Set<T>().CountAsync(predicate);
            return await _dbContext.Set<T>().CountAsync();
        }
    }
}
