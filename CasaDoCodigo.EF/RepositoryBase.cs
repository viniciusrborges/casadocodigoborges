using CasaDoCodigo.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CasaDoCodigo.EF
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
         where TEntity : class
    {
        protected readonly CasaDoCodigoDbContext _dbContext;
        protected readonly Expression<Func<TEntity, TKey>> _keySelector;
        
        public virtual IQueryable<TEntity> Query => Set().AsQueryable();

        internal RepositoryBase(CasaDoCodigoDbContext dbContext, Expression<Func<TEntity, TKey>> keySelector)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        }
      
        public async Task<TEntity> GetAsync(TKey key)
        {
            var body = Expression.Equal(_keySelector.Body, Expression.Constant(key));
            var expr = Expression.Lambda<Func<TEntity, bool>>(body, _keySelector.Parameters);

            return await Query.FirstAsync(expr);
        }

        public async Task AddAsync(TEntity entity) => await Set().AddAsync(entity);
        public async Task<List<TEntity>> FetchAllAsync() => await Set().ToListAsync();
        public async Task<int> SaveAsync() => await _dbContext.SaveChangesAsync();

        public void Remove(TEntity entity) => Set().Remove(entity);
        public void Update(TEntity entity) => Set().Update(entity);

        protected DbSet<TEntity> Set() => _dbContext.Set<TEntity>();
    }
}
